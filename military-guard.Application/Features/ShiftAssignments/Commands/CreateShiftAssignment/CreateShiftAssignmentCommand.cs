using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.ShiftAssignments.Commands.CreateShiftAssignment
{
    public record CreateShiftAssignmentCommand(
    Guid MilitiaId,
    Guid GuardPostId,
    Guid DutyShiftId,
    DateOnly Date,
    bool IsLeader
) : IRequest<Guid>;
}
