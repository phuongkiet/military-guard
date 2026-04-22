using Microsoft.Extensions.Logging;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Infrastructure.Services
{
    public class ShiftSchedulingService : IShiftSchedulingService
    {
        private readonly IMilitiaRepository _militiaRepo;
        private readonly IShiftAssignmentRepository _assignmentRepo;
        private readonly IDutyShiftRepository _dutyShiftRepo;
        private readonly IGuardPostRepository _guardPostRepo;
        private readonly ILeaveRequestRepository _leaveRepo;
        private readonly IGenericRepository<HolidayEvent> _holidayRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ShiftSchedulingService> _logger;

        public ShiftSchedulingService(
            IMilitiaRepository militiaRepo,
            IShiftAssignmentRepository assignmentRepo,
            IDutyShiftRepository dutyShiftRepo,
            IGuardPostRepository guardPostRepo,
            ILeaveRequestRepository leaveRepo,
            IGenericRepository<HolidayEvent> holidayRepo,
            IUnitOfWork unitOfWork,
            ILogger<ShiftSchedulingService> logger)
        {
            _militiaRepo = militiaRepo;
            _assignmentRepo = assignmentRepo;
            _dutyShiftRepo = dutyShiftRepo;
            _guardPostRepo = guardPostRepo;
            _leaveRepo = leaveRepo;
            _holidayRepo = holidayRepo;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task GenerateWeeklyScheduleAsync()
        {
            _logger.LogInformation("Bắt đầu chạy thuật toán xếp lịch tự động cho tuần tới...");

            // 1. Xác định mốc thời gian (Thứ 2 đến Chủ Nhật tuần sau)
            var today = DateOnly.FromDateTime(DateTime.Now);
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
            if (daysUntilMonday == 0) daysUntilMonday = 7;

            var nextMonday = today.AddDays(daysUntilMonday);
            var nextSunday = nextMonday.AddDays(6);

            // 2. Tải toàn bộ Data nền lên RAM để xử lý cho nhanh
            var allPosts = (await _guardPostRepo.GetAllAsync()).Where(p => p.IsActive).ToList();
            var allShifts = await _dutyShiftRepo.GetAllShiftsAsync();

            // 💡 RULE: Bỏ qua Chỉ huy trưởng và Chỉ huy phó (Họ không đi gác chốt)
            var allMilitias = (await _militiaRepo.GetAllAsync())
                .Where(m => !m.IsDeleted
                         && m.Rank != MilitiaRank.Commander
                         && m.Rank != MilitiaRank.ViceCommander)
                .ToList();

            var allHolidays = await _holidayRepo.GetAllAsync();

            // 3. Phân loại Lực lượng
            var standingForces = allMilitias.Where(m => m.Type == MilitiaType.Regular).ToList();
            var mobileForces = allMilitias.Where(m => m.Type == MilitiaType.Mobile).ToList();

            // 💡 CÔNG CỤ: Bộ nhớ đệm giúp ngăn 1 người trực 2 chốt cùng giờ
            var memoryTracker = new HashSet<string>();

            // 💡 TÌM TRỤ SỞ: Tìm chốt có tên chứa chữ "Trụ sở" để nhét quân dự bị vào
            var hqPost = allPosts.FirstOrDefault(p => p.Name.Contains("Trụ sở"));

            // 4. BẮT ĐẦU VÒNG LẶP THỜI GIAN
            for (var date = nextMonday; date <= nextSunday; date = date.AddDays(1))
            {
                bool isHoliday = allHolidays.Any(h => h.Date == date && h.RequiresMobileForces);

                foreach (var shift in allShifts)
                {
                    // Lấy danh sách những người đang xin nghỉ phép trong ngày này
                    var onLeaveIds = (await _leaveRepo.GetAllAsync())
                        .Where(l => l.Status == LeaveStatus.Approved &&
                                    DateOnly.FromDateTime(l.StartDate) <= date &&
                                    DateOnly.FromDateTime(l.EndDate) >= date)
                        .Select(l => l.MilitiaId).ToHashSet();

                    bool isEveningShift = shift.StartTime >= new TimeOnly(18, 0);

                    foreach (var post in allPosts)
                    {
                        // Nếu là chốt "Trụ sở" thì bỏ qua, để dành xử lý sau
                        if (hqPost != null && post.Id == hqPost.Id) continue;

                        List<Militia> candidatePool = new List<Militia>();

                        if (isHoliday)
                        {
                            candidatePool.AddRange(mobileForces);
                            candidatePool.AddRange(standingForces);
                        }
                        else if (isEveningShift)
                        {
                            candidatePool.AddRange(mobileForces);
                            candidatePool.AddRange(standingForces);
                        }
                        else
                        {
                            candidatePool.AddRange(standingForces);
                        }

                        // Lọc người bận và Randomize để xếp ca công bằng
                        var availableMilitias = candidatePool
                            .Where(m => !onLeaveIds.Contains(m.Id))
                            .OrderBy(x => Guid.NewGuid())
                            .ToList();

                        // 💡 GIẢI PHÁP MỚI: Bốc quân vào 1 đội trước
                        var selectedTeam = new List<Militia>();
                        bool hasCapableLeader = false;

                        foreach (var militia in availableMilitias)
                        {
                            // Nếu đã đủ người tối đa thì ngưng bốc
                            if (selectedTeam.Count >= post.MaxPersonnel) break;

                            string trackingKey = $"{militia.Id}_{date:yyyyMMdd}_{shift.Id}";

                            bool isDoubleBookedDb = await _assignmentRepo.IsMilitiaDoubleBookedAsync(militia.Id, date, shift.Id);

                            if (isDoubleBookedDb || memoryTracker.Contains(trackingKey))
                                continue;

                            // Thêm vào team và đánh dấu đã bốc
                            selectedTeam.Add(militia);
                            memoryTracker.Add(trackingKey);

                            // Đánh dấu xem trong team đã có ai CÓ THỂ làm leader chưa
                            if (militia.IsCapableLeader()) hasCapableLeader = true;

                            // Nếu đã đạt MinPersonnel VÀ trong team đã có người đủ chuẩn làm leader thì dừng bốc
                            if (selectedTeam.Count >= post.MinPersonnel && hasCapableLeader) break;
                        }

                        var leader = selectedTeam
                            .Where(m => m.IsCapableLeader())
                            .OrderByDescending(m => m.Rank) // Cấp bậc to nhất lên đầu
                            .ThenBy(m => m.JoinDate)        // Vào ngành sớm nhất lên đầu
                            .FirstOrDefault();

                        // 💡 RÓT QUÂN XUỐNG DB
                        foreach (var militia in selectedTeam)
                        {
                            var assignment = new ShiftAssignment
                            {
                                Id = Guid.NewGuid(),
                                Date = date,
                                DutyShiftId = shift.Id,
                                GuardPostId = post.Id,
                                MilitiaId = militia.Id,
                                IsLeader = (leader != null && militia.Id == leader.Id), // Chỉ gán true cho đúng ông leader đã bầu
                                IsStandby = false,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = "System_Auto"
                            };

                            await _assignmentRepo.AddAsync(assignment);
                        }
                    }

                    if (hqPost != null)
                    {
                        // Ngày lễ gọi 5 dự bị, Ca tối gọi 2 dự bị, Ca ngày không gọi
                        int requiredStandby = isHoliday ? 5 : (isEveningShift ? 2 : 0);

                        if (requiredStandby > 0)
                        {
                            // Quét lấy những anh Cơ động CHƯA bị bắt đi gác chốt ở Phase A
                            var availableStandby = mobileForces
                                .Where(m => !onLeaveIds.Contains(m.Id)) // Không xin nghỉ phép
                                .Where(m => !memoryTracker.Contains($"{m.Id}_{date:yyyyMMdd}_{shift.Id}")) // Đang rảnh
                                .OrderBy(x => Guid.NewGuid())
                                .Take(requiredStandby)
                                .ToList();

                            foreach (var militia in availableStandby)
                            {
                                var standbyAssignment = new ShiftAssignment
                                {
                                    Id = Guid.NewGuid(),
                                    Date = date,
                                    DutyShiftId = shift.Id,
                                    GuardPostId = hqPost.Id,
                                    MilitiaId = militia.Id,
                                    IsLeader = false, // Quân dự bị không cần trưởng ca
                                    IsStandby = true, // 💡 Bật cờ dự bị lên
                                    CreatedAt = DateTime.UtcNow,
                                    CreatedBy = "System_Auto_Standby"
                                };

                                await _assignmentRepo.AddAsync(standbyAssignment);
                                memoryTracker.Add($"{militia.Id}_{date:yyyyMMdd}_{shift.Id}");
                            }
                        }
                    }
                }
            }

            // Lưu tất cả một lần duy nhất xuống DB
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation($"Đã tạo xong lịch trực tự động cho tuần từ {nextMonday} đến {nextSunday}.");
        }

        public async Task ScanAndNotifyUpcomingShiftsAsync()
        {
            _logger.LogInformation("Đang quét để gửi thông báo 36h...");
            // TODO: Viết logic thông báo ở các phần sau
            await Task.CompletedTask;
        }
    }
}
