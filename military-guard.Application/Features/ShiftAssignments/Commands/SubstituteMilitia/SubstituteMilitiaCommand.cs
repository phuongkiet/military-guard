using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.ShiftAssignments.Commands.SubstituteMilitia
{
    public class SubstituteMilitiaCommand : IRequest<Guid>
    {
        public Guid AbsentAssignmentId { get; set; } 
        public Guid SubstituteMilitiaId { get; set; }
    }
}
