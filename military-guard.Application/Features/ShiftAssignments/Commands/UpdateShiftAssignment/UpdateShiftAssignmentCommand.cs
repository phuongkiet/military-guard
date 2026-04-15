using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.ShiftAssignments.Commands.UpdateShiftAssignment
{
    public class UpdateShiftAssignmentCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public bool IsLeader { get; set; }
    }
}
