using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.HolidayEvents.DTOs
{
    public record HolidayEventResponse
    (
        Guid Id,
        string Name,
        bool RequiresMobileForces,
        string? Description
    );
}
