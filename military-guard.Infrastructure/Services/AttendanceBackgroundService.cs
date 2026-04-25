using Microsoft.Extensions.Logging;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Infrastructure.Services
{
    public class AttendanceBackgroundService : IAttendanceBackgroundService
    {
        private readonly IShiftAssignmentRepository _assignmentRepo;
        private readonly IAttendanceRepository _attendanceRepo;
        private readonly ISignalRService _signalRService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AttendanceBackgroundService> _logger;

        public AttendanceBackgroundService(
            IShiftAssignmentRepository assignmentRepo,
            IAttendanceRepository attendanceRepo,
            ISignalRService signalRService,
            IUnitOfWork unitOfWork,
            ILogger<AttendanceBackgroundService> logger)
        {
            _assignmentRepo = assignmentRepo;
            _attendanceRepo = attendanceRepo;
            _signalRService = signalRService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task AutoCloseAttendanceAsync()
        {
            _logger.LogInformation("Trọng tài đang quét kiểm tra điểm danh...");

            var now = DateTime.Now;
            var today = DateOnly.FromDateTime(now);
            var currentTime = TimeOnly.FromDateTime(now);

            // 1. Lấy toàn bộ ca trực CỦA NGÀY HÔM NAY (Dùng hàm phân trang lấy max 1000 record cho lẹ)
            var pagedAssignments = await _assignmentRepo.GetPagedAssignmentsAsync(today, null, null, 1, 1000);
            var assignmentsToday = pagedAssignments.Items;

            // 2. Lấy toàn bộ lịch sử điểm danh để đối chiếu
            var allAttendances = await _attendanceRepo.GetAllAsync();
            var todayAttendances = allAttendances.Where(a => DateOnly.FromDateTime(a.CheckInTime) == today).ToList();

            bool hasChanges = false;

            // 3. Bắt đầu đi tuần tra từng người một
            foreach (var assignment in assignmentsToday)
            {
                var shiftStartTime = assignment.DutyShift!.StartTime;
                var deadlineTime = shiftStartTime.AddMinutes(10); // Giờ G + 10 phút

                // Nếu thời gian hiện tại đã QUA 10 phút ân hạn VÀ ca trực VẪN CHƯA KẾT THÚC
                if (currentTime > deadlineTime && currentTime < assignment.DutyShift.EndTime)
                {
                    // Hỏi xem ông này đã điểm danh chưa?
                    bool hasCheckedIn = todayAttendances.Any(a => a.MilitiaId == assignment.MilitiaId && a.ShiftId == assignment.DutyShiftId);

                    if (!hasCheckedIn)
                    {
                        _logger.LogWarning($"Phát hiện Dân quân {assignment.Militia?.FullName} vắng mặt ở chốt {assignment.GuardPost?.Name}");

                        // Tạo ngay 1 record Đánh vắng mặt
                        var absentRecord = new Attendance
                        {
                            Id = Guid.NewGuid(),
                            MilitiaId = assignment.MilitiaId,
                            ShiftId = assignment.DutyShiftId,
                            CheckInTime = now, // Ghi nhận thời gian trọng tài thổi còi
                            Status = AttendanceStatus.Absent,
                            Note = "Hệ thống đã chốt sổ vắng mặt."
                        };

                        await _attendanceRepo.AddAsync(absentRecord);

                        // Cập nhật RAM tạm để lỡ vòng lặp có quét lại thì không bị trùng
                        todayAttendances.Add(absentRecord);
                        hasChanges = true;

                        // 🔴 KÍCH HOẠT BÁO ĐỘNG ĐỎ QUA SIGNALR CHO CHỈ HUY
                        string militiaName = assignment.Militia?.FullName ?? "Unknown";

                        // 1. Báo ra bảng tin chung
                        await _signalRService.SendAttendanceNotification(
                            militiaName, now, AttendanceStatus.Absent.ToString(), isEmergency: true
                        );

                        // 2. Báo vào đúng phòng của ca đó
                        await _signalRService.SendCheckInEventToRoom(
                            assignment.DutyShiftId, today, assignment.MilitiaId, AttendanceStatus.Absent, now
                        );
                    }
                }
            }

            if (hasChanges)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("Đã lưu các trường hợp vắng mặt xuống Database.");
            }
        }
    }
}
