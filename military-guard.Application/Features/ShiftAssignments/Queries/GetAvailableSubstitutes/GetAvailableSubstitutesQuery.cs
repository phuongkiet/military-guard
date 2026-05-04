using MediatR;
using military_guard.Application.Features.ShiftAssignments.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.ShiftAssignments.Queries.GetAvailableSubstitutes
{
    public class GetAvailableSubstitutesQuery : IRequest<List<SubstituteDto>>
    {
        public Guid AbsentAssignmentId { get; set; }
    }
}
