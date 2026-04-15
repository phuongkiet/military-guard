using MediatR;
using military_guard.Application.Common.Models;
using military_guard.Application.Features.ShiftAssignments.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.ShiftAssignments.Queries.GetAllShiftAssignments
{
    public record GetAllShiftAssignmentsQuery : IRequest<PaginatedList<ShiftAssignmentResponse>>
    {
        public DateOnly? Date { get; init; }
        public Guid? GuardPostId { get; init; }
        public Guid? MilitiaId { get; init; }
        public int PageIndex { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
