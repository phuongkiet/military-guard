using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Attendances.DTOs
{
    public class LiveAttendanceDto
    {
        public Guid AssignmentId { get; set; }
        public Guid MilitiaId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string GuardPostName { get; set; } = string.Empty;
        public string DutyShiftName { get; set; } = string.Empty;
        public bool IsLeader { get; set; }
        public bool IsStandby { get; set; }
        public string? Note { get; set; }

        public Guid? AttendanceId { get; set; }
        public DateTime? CheckInTime { get; set; }
        public AttendanceStatus? Status { get; set; }
    }
}
