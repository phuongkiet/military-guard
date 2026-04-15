using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.DutyShifts.DTOs
{
    public record DutyShiftResponse(Guid Id, TimeOnly StartTime, TimeOnly EndTime, int ShiftOrder);
}
