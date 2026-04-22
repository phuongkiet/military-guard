using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.HolidayEvents.Commands.CreateHolidayEvent
{
    public record CreateHolidayEventCommand
    (
        string Name,
        DateOnly Date,
        bool RequiresMobileForces,
        string? Description
    ) : IRequest<Guid>;
}
