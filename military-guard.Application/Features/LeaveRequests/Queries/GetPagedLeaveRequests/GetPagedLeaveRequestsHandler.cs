using MediatR;
using military_guard.Application.Common.Models;
using military_guard.Application.Features.LeaveRequests.DTOs;
using military_guard.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.LeaveRequests.Queries.GetPagedLeaveRequests
{
    public class GetPagedLeaveRequestsHandler : IRequestHandler<GetPagedLeaveRequestsQuery, PaginatedList<LeaveRequestResponse>>
    {
        private readonly ILeaveRequestRepository _repository;

        public GetPagedLeaveRequestsHandler(ILeaveRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<LeaveRequestResponse>> Handle(GetPagedLeaveRequestsQuery request, CancellationToken cancellationToken)
        {
            var pagedEntities = await _repository.GetPagedLeaveRequestsAsync(
                request.MilitiaId, request.Status, request.PageIndex, request.PageSize);

            var dtos = pagedEntities.Items.Select(l => new LeaveRequestResponse(
                Id: l.Id,
                MilitiaId: l.MilitiaId,
                MilitiaName: l.Militia?.FullName ?? "N/A", 
                StartDate: l.StartDate,
                EndDate: l.EndDate,
                Reason: l.Reason,
                Status: l.Status.ToString() 
            )).ToList();

            return new PaginatedList<LeaveRequestResponse>(
                dtos, pagedEntities.TotalCount, request.PageIndex, request.PageSize);
        }
    }
}
