using MediatR;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.ShiftAssignments.Commands.SubstituteMilitia
{
    public class SubstituteMilitiaHandler : IRequestHandler<SubstituteMilitiaCommand, Guid>
    {
        private readonly IShiftAssignmentRepository _assignmentRepo;
        private readonly IAttendanceRepository _attendanceRepo;
        private readonly IMilitiaRepository _militiaRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISignalRService _signalRService;

        public SubstituteMilitiaHandler(
            IShiftAssignmentRepository assignmentRepo,
            IAttendanceRepository attendanceRepo,
            IMilitiaRepository militiaRepo,
            IUnitOfWork unitOfWork,
            ISignalRService signalRService)
        {
            _assignmentRepo = assignmentRepo;
            _attendanceRepo = attendanceRepo;
            _militiaRepo = militiaRepo;
            _unitOfWork = unitOfWork;
            _signalRService = signalRService;
        }

        public async Task<Guid> Handle(SubstituteMilitiaCommand request, CancellationToken cancellationToken)
        {
            var absentAssignment = await _assignmentRepo.GetByIdAsync(request.AbsentAssignmentId);
            if (absentAssignment == null) throw new Exception("Không tìm thấy phân công trực gốc.");

            var substituteMilitia = await _militiaRepo.GetByIdAsync(request.SubstituteMilitiaId);
            if (substituteMilitia == null) throw new Exception("Không tìm thấy thông tin dân quân thay thế.");

            Guid resultAssignmentId;

            var existingAssignment = await _assignmentRepo.GetByMilitiaAndShiftAsync(
                request.SubstituteMilitiaId,
                absentAssignment.DutyShiftId,
                absentAssignment.Date);

            if (existingAssignment != null)
            {
                existingAssignment.GuardPostId = absentAssignment.GuardPostId;
                existingAssignment.IsStandby = false;
                existingAssignment.UpdatedAt = DateTime.UtcNow;

                _assignmentRepo.UpdateAsync(existingAssignment);
                resultAssignmentId = existingAssignment.Id;
            }
            else
            {
                var newAssignment = new ShiftAssignment
                {
                    Id = Guid.NewGuid(),
                    MilitiaId = request.SubstituteMilitiaId,
                    GuardPostId = absentAssignment.GuardPostId,
                    DutyShiftId = absentAssignment.DutyShiftId,
                    Date = absentAssignment.Date,
                    IsLeader = absentAssignment.IsLeader,
                    IsStandby = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _assignmentRepo.AddAsync(newAssignment);
                resultAssignmentId = newAssignment.Id;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _signalRService.SendCheckInEventToRoom(
                absentAssignment.DutyShiftId,
                absentAssignment.Date,
                request.SubstituteMilitiaId,
                AttendanceStatus.OnTime,
                DateTime.Now
            );

            await _signalRService.SendShiftStructureChangedEvent(
                absentAssignment.DutyShiftId,
                absentAssignment.Date
            );

            return resultAssignmentId;
        }
    }
}
