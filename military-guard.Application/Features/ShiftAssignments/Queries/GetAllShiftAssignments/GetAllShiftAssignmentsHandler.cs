using MediatR;
using military_guard.Application.Common.Models;
using military_guard.Application.Features.ShiftAssignments.DTOs;
using military_guard.Application.Interfaces;

namespace military_guard.Application.Features.ShiftAssignments.Queries.GetAllShiftAssignments
{
    public class GetAllShiftAssignmentsHandler : IRequestHandler<GetAllShiftAssignmentsQuery, PaginatedList<ShiftAssignmentResponse>>
    {
        private readonly IShiftAssignmentRepository _shiftAssignmentRepository;

        public GetAllShiftAssignmentsHandler(IShiftAssignmentRepository shiftAssignmentRepository)
        {
            _shiftAssignmentRepository = shiftAssignmentRepository;
        }

        public async Task<PaginatedList<ShiftAssignmentResponse>> Handle(GetAllShiftAssignmentsQuery request, CancellationToken cancellationToken)
        {
            var pagedEntities = await _shiftAssignmentRepository.GetPagedAssignmentsAsync(
                request.Date, request.GuardPostId, request.MilitiaId, request.PageIndex, request.PageSize);

            var dtos = pagedEntities.Items.Select(sa => new ShiftAssignmentResponse(
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
            )).ToList();

            return new PaginatedList<ShiftAssignmentResponse>(dtos, pagedEntities.TotalCount, request.PageIndex, request.PageSize);
        }
    }
}
