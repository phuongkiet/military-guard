using MediatR;
using military_guard.Application.Features.DutyShifts.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.DutyShifts.Queries.GetDutyShiftById
{
    public record GetDutyShiftByIdQuery(Guid Id) : IRequest<DutyShiftResponse>;
}
