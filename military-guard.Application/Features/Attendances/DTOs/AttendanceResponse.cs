using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Attendances.DTOs
{
    public record AttendanceResponse(
    AttendanceStatus Status
);
}
