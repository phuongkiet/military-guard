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

            var today = DateOnly.FromDateTime(DateTime.Now);
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
            if (daysUntilMonday == 0) daysUntilMonday = 7;

            var nextMonday = today.AddDays(daysUntilMonday);
            var nextSunday = nextMonday.AddDays(6);

            var allPosts = (await _guardPostRepo.GetAllAsync()).Where(p => p.IsActive).ToList();
            var allShifts = await _dutyShiftRepo.GetAllShiftsAsync();
            var allMilitias = (await _militiaRepo.GetAllAsync())
                .Where(m => !m.IsDeleted
                         && m.Rank != MilitiaRank.Commander
                         && m.Rank != MilitiaRank.ViceCommander) // Chặn đứng tại đây
                .ToList();
            var allHolidays = await _holidayRepo.GetAllAsync();

            var standingForces = allMilitias.Where(m => m.Type == MilitiaType.Regular).ToList();
            var mobileForces = allMilitias.Where(m => m.Type == MilitiaType.Mobile).ToList();

            // 💡 FIX LỖI: Tạo 1 bộ nhớ tạm để "nhớ" những ai đã được xếp ca trong đợt chạy này
            var memoryTracker = new HashSet<string>();

            for (var date = nextMonday; date <= nextSunday; date = date.AddDays(1))
            {
                bool isHoliday = allHolidays.Any(h => h.Date == date && h.RequiresMobileForces);

                foreach (var post in allPosts)
                {
                    foreach (var shift in allShifts)
                    {
                        var onLeaveIds = (await _leaveRepo.GetAllAsync())
                            .Where(l => l.Status == LeaveStatus.Approved &&
                                        DateOnly.FromDateTime(l.StartDate) <= date &&
                                        DateOnly.FromDateTime(l.EndDate) >= date)
                            .Select(l => l.MilitiaId).ToHashSet();

                        bool isEveningShift = shift.StartTime >= new TimeOnly(18, 0);

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

                        var availableMilitias = candidatePool
                            .Where(m => !onLeaveIds.Contains(m.Id))
                            .OrderBy(x => Guid.NewGuid())
                            .ToList();

                        int assignedCount = 0;
                        bool hasLeader = false;

                        foreach (var militia in availableMilitias)
                        {
                            if (assignedCount >= post.MaxPersonnel) break;

                            // 💡 FIX LỖI: Tạo chìa khóa (Key) để check trùng lặp (Ví dụ: "MilitiaId_20260503_ShiftId")
                            string trackingKey = $"{militia.Id}_{date:yyyyMMdd}_{shift.Id}";

                            // Check xem DB có lịch cũ không?
                            bool isDoubleBookedDb = await _assignmentRepo.IsMilitiaDoubleBookedAsync(militia.Id, date, shift.Id);

                            // Nếu DB đã có, HOẶC vòng lặp nãy (ở chốt khác) đã pick anh này rồi thì Bỏ Qua!
                            if (isDoubleBookedDb || memoryTracker.Contains(trackingKey))
                                continue;

                            bool assignAsLeader = false;
                            if (!hasLeader && militia.IsCapableLeader())
                            {
                                assignAsLeader = true;
                                hasLeader = true;
                            }

                            if (assignedCount >= post.MinPersonnel && hasLeader) break;

                            var assignment = new ShiftAssignment
                            {
                                Id = Guid.NewGuid(),
                                Date = date,
                                DutyShiftId = shift.Id,
                                GuardPostId = post.Id,
                                MilitiaId = militia.Id,
                                IsLeader = assignAsLeader,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = "System_Auto"
                            };

                            await _assignmentRepo.AddAsync(assignment);

                            // 💡 FIX LỖI: Đánh dấu anh này vào RAM là "ĐÃ CÓ CA TRONG NGÀY NÀY, GIỜ NÀY RỒI NHÉ!"
                            memoryTracker.Add(trackingKey);

                            assignedCount++;
                        }
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation($"Đã tạo xong lịch trực tự động cho tuần từ {nextMonday} đến {nextSunday}.");
        }

        public async Task ScanAndNotifyUpcomingShiftsAsync()
        {
            // TODO: Phần bắn SignalR 36h anh em mình sẽ ráp sau khi lịch đã chạy mượt.
            _logger.LogInformation("Đang quét để gửi thông báo...");
            await Task.CompletedTask;
        }
    }
}
