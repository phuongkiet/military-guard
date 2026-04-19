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

            var shift = await _dutyShiftRepository.GetByIdAsync(request.ShiftId);
            if (shift == null) throw new Exception("Không tìm thấy thông tin ca trực.");

            var militia = await _militiaRepository.GetByIdAsync(request.MilitiaId);
            if (militia == null) throw new Exception("Không tìm thấy thông tin dân quân.");

            var status = AttendanceStatus.OnTime;

            var currentTimeOnly = TimeOnly.FromDateTime(now);

            if (currentTimeOnly > shift.StartTime)
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

            await _signalRService.SendAttendanceNotification(
                militia.FullName,
                now,
                status.ToString(),
                status == AttendanceStatus.PenaltyThreshold
            );

            return new AttendanceResponse(status);
        }
    }
}
