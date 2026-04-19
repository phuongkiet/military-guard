using military_guard.Domain.Entities.Common;
using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Domain.Entities
{
    public class Attendance : BaseEntity
    {
        public Guid MilitiaId { get; set; }
        public Guid ShiftId { get; set; } // Liên kết tới ca trực được phân công
        public DateTime CheckInTime { get; set; }
        public AttendanceStatus Status { get; set; }
        public string? Note { get; set; } // Lý do nếu trễ

        public virtual Militia Militia { get; set; }
    }
}
