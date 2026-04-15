using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.ShiftAssignments.Commands.DeleteShiftAssignment
{
    public record DeleteShiftAssignmentCommand(Guid Id) : IRequest<Unit>;
}
