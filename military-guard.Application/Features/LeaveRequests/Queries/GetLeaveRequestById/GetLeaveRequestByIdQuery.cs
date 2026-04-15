using MediatR;
using military_guard.Application.Features.LeaveRequests.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.LeaveRequests.Queries.GetLeaveRequestById
{
    public record GetLeaveRequestByIdQuery(Guid Id) : IRequest<LeaveRequestResponse>;
}
