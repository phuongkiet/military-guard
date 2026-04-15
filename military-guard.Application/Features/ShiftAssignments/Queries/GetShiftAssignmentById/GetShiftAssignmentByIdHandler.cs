using MediatR;
using military_guard.Application.Features.ShiftAssignments.DTOs;
using military_guard.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.ShiftAssignments.Queries.GetShiftAssignmentById
{
    public class GetShiftAssignmentByIdHandler : IRequestHandler<GetShiftAssignmentByIdQuery, ShiftAssignmentResponse>
    {
        private readonly IShiftAssignmentRepository _repository;

        public GetShiftAssignmentByIdHandler(IShiftAssignmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<ShiftAssignmentResponse> Handle(GetShiftAssignmentByIdQuery request, CancellationToken cancellationToken)
        {
            var sa = await _repository.GetAssignmentDetailsAsync(request.Id);

            if (sa == null)
                throw new KeyNotFoundException($"Không tìm thấy phân công với ID: {request.Id}");

            return new ShiftAssignmentResponse(
                Id: sa.Id,
                MilitiaId: sa.MilitiaId,
                MilitiaName: sa.Militia?.FullName ?? "N/A",
                MilitiaRank: sa.Militia?.Rank.ToString() ?? "",
                GuardPostId: sa.GuardPostId,
                GuardPostName: sa.GuardPost?.Name ?? "N/A",
                DutyShiftId: sa.DutyShiftId,
                DutyShiftInfo: sa.DutyShift != null ? $"Ca {sa.DutyShift.ShiftOrder} ({sa.DutyShift.StartTime} - {sa.DutyShift.EndTime})" : "N/A",
                Date: sa.Date,
                IsLeader: sa.IsLeader
            );
        }
    }
}
