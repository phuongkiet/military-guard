using MediatR;
using military_guard.Application.Features.ShiftAssignments.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.ShiftAssignments.Queries.GetShiftAssignmentById
{
    public record GetShiftAssignmentByIdQuery(Guid Id) : IRequest<ShiftAssignmentResponse>;
}
