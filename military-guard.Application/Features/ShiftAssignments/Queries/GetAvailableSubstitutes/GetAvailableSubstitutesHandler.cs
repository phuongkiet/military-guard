using MediatR;
using military_guard.Application.Features.ShiftAssignments.DTOs;
using military_guard.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.ShiftAssignments.Queries.GetAvailableSubstitutes
{
    public class GetAvailableSubstitutesHandler : IRequestHandler<GetAvailableSubstitutesQuery, List<SubstituteDto>>
    {
        private readonly IShiftAssignmentRepository _assignmentRepo;
        private readonly IMilitiaRepository _militiaRepo;

        public GetAvailableSubstitutesHandler(IShiftAssignmentRepository assignmentRepo, IMilitiaRepository militiaRepo)
        {
            _assignmentRepo = assignmentRepo;
            _militiaRepo = militiaRepo;
        }

        public async Task<List<SubstituteDto>> Handle(GetAvailableSubstitutesQuery request, CancellationToken cancellationToken)
        {
            var absentAssignment = await _assignmentRepo.GetByIdAsync(request.AbsentAssignmentId);
            if (absentAssignment == null)
            {
                throw new Exception("Không tìm thấy thông tin ca trực bị vắng.");
            }

            var availableList = await _militiaRepo.GetAvailableForShiftAsync(absentAssignment.DutyShiftId, absentAssignment.Date);

            availableList.RemoveAll(x => x.MilitiaId == absentAssignment.MilitiaId);

            return availableList;
        }
    }
}
