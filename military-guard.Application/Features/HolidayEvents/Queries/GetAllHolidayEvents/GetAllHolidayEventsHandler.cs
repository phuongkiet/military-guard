using MediatR;
using Microsoft.IdentityModel.Tokens;
using military_guard.Application.Common.Models;
using military_guard.Application.Features.DutyShifts.DTOs;
using military_guard.Application.Features.HolidayEvents.DTOs;
using military_guard.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.HolidayEvents.Queries.GetAllHolidayEvents
{
    public class GetAllHolidayEventsHandler : IRequestHandler<GetAllHolidayEventsQuery, PaginatedList<HolidayEventResponse>>
    {
        private readonly IHolidayEventRepository _holidayEventRepository;

        public GetAllHolidayEventsHandler(IHolidayEventRepository holidayEventRepository)
        {
            _holidayEventRepository = holidayEventRepository;
        }

        public async Task<PaginatedList<HolidayEventResponse>> Handle(GetAllHolidayEventsQuery request, CancellationToken cancellationToken)
        {
            var holidayEvents = await _holidayEventRepository.GetPagedHolidyEventsAsync(request.PageIndex, request.PageSize);

            var dtos = holidayEvents.Items.Select(events => new HolidayEventResponse(
                Id: events.Id,
                Name: events.Name,
                RequiresMobileForces: events.RequiresMobileForces,
                Description: events.Description ?? "Không có"
                )).ToList();

            return new PaginatedList<HolidayEventResponse>(dtos, holidayEvents.TotalCount, request.PageIndex, request.PageSize);
        }
    }
}
