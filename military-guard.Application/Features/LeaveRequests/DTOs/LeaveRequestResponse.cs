using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.LeaveRequests.DTOs
{
    public record LeaveRequestResponse(
    Guid Id,
    Guid MilitiaId,
    string MilitiaName,
    DateTime StartDate,
    DateTime EndDate,
    string Reason,
    string Status
);
}
