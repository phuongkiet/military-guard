using military_guard.Domain.Entities.Common;
using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Domain.Entities
{
    public class LeaveRequest : BaseEntity
    {
        // Ai xin nghỉ?
        public Guid MilitiaId { get; set; }
        public Militia? Militia { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

        // Ai duyệt? (Chỉ huy phó / Chỉ huy trưởng)
        public Guid? ApproverId { get; set; }
        public Militia? Approver { get; set; }
    }
}
