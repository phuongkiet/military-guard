using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.LeaveRequests.Commands.CreateLeaveRequest
{
    public record CreateLeaveRequestCommand(
    Guid MilitiaId,
    DateTime StartDate,
    DateTime EndDate,
    string Reason
) : IRequest<Guid>;
}
