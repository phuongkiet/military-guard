using MediatR;
using military_guard.Application.Common.Models;
using military_guard.Application.Features.HolidayEvents.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.HolidayEvents.Queries.GetAllHolidayEvents
{
    public record GetAllHolidayEventsQuery : IRequest<PaginatedList<HolidayEventResponse>>
    {
        public int PageIndex { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
