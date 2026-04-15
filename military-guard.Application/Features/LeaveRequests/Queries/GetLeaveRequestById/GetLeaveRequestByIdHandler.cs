using MediatR;
using military_guard.Application.Features.LeaveRequests.DTOs;
using military_guard.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.LeaveRequests.Queries.GetLeaveRequestById
{
    public class GetLeaveRequestByIdHandler : IRequestHandler<GetLeaveRequestByIdQuery, LeaveRequestResponse>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public GetLeaveRequestByIdHandler(ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
        }

        public async Task<LeaveRequestResponse> Handle(GetLeaveRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestDetailsAsync(request.Id);

            if (leaveRequest == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn xin nghỉ phép với ID: {request.Id}");
            }

            return new LeaveRequestResponse(
                Id: leaveRequest.Id,
                MilitiaId: leaveRequest.MilitiaId,
                MilitiaName: leaveRequest.Militia?.FullName ?? "N/A",
                StartDate: leaveRequest.StartDate,
                EndDate: leaveRequest.EndDate,
                Reason: leaveRequest.Reason,
                Status: leaveRequest.Status.ToString()
            );
        }
    }
}
