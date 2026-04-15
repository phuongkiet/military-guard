using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.LeaveRequests.Commands.DeleteLeaveRequest
{
    public record DeleteLeaveRequestCommand(Guid Id) : IRequest<Unit>;
}
