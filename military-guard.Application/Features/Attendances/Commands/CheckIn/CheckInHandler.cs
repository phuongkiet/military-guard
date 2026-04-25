using MediatR;
using Microsoft.AspNetCore.SignalR;
using military_guard.Application.Features.Attendances.DTOs;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using military_guard.Domain.Enums;

namespace military_guard.Application.Features.Attendances.Commands.CheckIn
{
    public class CheckInHandler : IRequestHandler<CheckInCommand, AttendanceResponse>
    {
        private readonly IDutyShiftRepository _dutyShiftRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMilitiaRepository _militiaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISignalRService _signalRService;

        public CheckInHandler(
            IDutyShiftRepository dutyShiftRepository,
            IAttendanceRepository attendanceRepository,
            IMilitiaRepository militiaRepository,
            IUnitOfWork unitOfWork,
            ISignalRService signalRService)
        {
            _dutyShiftRepository = dutyShiftRepository;
            _attendanceRepository = attendanceRepository;
            _militiaRepository = militiaRepository;
            _unitOfWork = unitOfWork;
            _signalRService = signalRService;
        }

        public async Task<AttendanceResponse> Handle(CheckInCommand request, CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            var currentTimeOnly = TimeOnly.FromDateTime(now);
            var today = DateOnly.FromDateTime(now);

            var shift = await _dutyShiftRepository.GetByIdAsync(request.ShiftId);
            if (shift == null) throw new Exception("Không tìm thấy thông tin ca trực.");

            var militia = await _militiaRepository.GetByIdAsync(request.MilitiaId);
            if (militia == null) throw new Exception("Không tìm thấy thông tin dân quân.");

            if (currentTimeOnly < shift.StartTime)
            {
                throw new Exception($"Chưa tới giờ điểm danh! Ca trực của đồng chí sẽ mở vào lúc {shift.StartTime:HH:mm}. Vui lòng chờ thêm {(shift.StartTime - currentTimeOnly).TotalMinutes:F0} phút nữa.");
            }

            var deadlineTime = shift.StartTime.AddMinutes(10);
            if (currentTimeOnly > deadlineTime)
            {
                throw new Exception($"Đã quá thời gian điểm danh! Ca trực mở lúc {shift.StartTime:HH:mm} và đã đóng cửa lúc {deadlineTime:HH:mm}. Đồng chí đã bị ghi nhận Vắng mặt.");
            }

            bool hasCheckedIn = await _attendanceRepository.HasCheckedInAsync(request.MilitiaId, request.ShiftId, today);
            if (hasCheckedIn) throw new Exception("Đồng chí đã điểm danh cho ca này rồi!");

            var status = AttendanceStatus.OnTime;
            if (currentTimeOnly > shift.StartTime.AddMinutes(10))
            {
                var previousWarnings = await _attendanceRepository.CountLateInMonth(request.MilitiaId, now);
                status = (previousWarnings >= 1) ? AttendanceStatus.PenaltyThreshold : AttendanceStatus.LateWarning;
            }

            var attendance = new Attendance
            {
                Id = Guid.NewGuid(),
                MilitiaId = request.MilitiaId,
                ShiftId = request.ShiftId,
                CheckInTime = now,
                Status = status
            };

            await _attendanceRepository.AddAsync(attendance);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (status == AttendanceStatus.PenaltyThreshold)
            {
                await _signalRService.SendAttendanceNotification(
                    militia.FullName,
                    now,
                    status.ToString(),
                    true
                );
            }

            await _signalRService.SendCheckInEventToRoom(
                shift.Id,
                today,
                request.MilitiaId,
                status,
                now
            );

            return new AttendanceResponse(attendance.Id, status);
        }
    }
}
