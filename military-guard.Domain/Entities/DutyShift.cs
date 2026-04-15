using military_guard.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Domain.Entities
{
    public class DutyShift : BaseEntity
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public int ShiftOrder { get; set; }

        // Navigation Property
        public ICollection<ShiftAssignment> ShiftAssignments { get; set; } = new List<ShiftAssignment>();
    }
}
