using military_guard.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Domain.Entities
{
    public class ShiftAssignment : BaseEntity
    {
        public Guid MilitiaId { get; set; }
        public Militia? Militia { get; set; }

        public Guid GuardPostId { get; set; }
        public GuardPost? GuardPost { get; set; }

        public Guid DutyShiftId { get; set; }
        public DutyShift? DutyShift { get; set; }

        public DateOnly Date { get; set; }

        public bool IsLeader { get; set; } = false;

        public bool IsStandby { get; set; } = false;
    }
}
