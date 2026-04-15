using military_guard.Domain.Entities.Common;
using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Domain.Entities
{
    public class Militia : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public MilitiaType Type { get; set; }
        public MilitiaRank Rank { get; set; }
        public DateTime JoinDate { get; set; }
        public Guid? ManagerId { get; set; }
        public Militia? Manager { get; set; }

        public ICollection<ShiftAssignment> ShiftAssignments { get; set; } = new List<ShiftAssignment>();
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

        public int MonthsOfService
        {
            get
            {
                var today = DateTime.UtcNow;
                int months = ((today.Year - JoinDate.Year) * 12) + today.Month - JoinDate.Month;
                return months < 0 ? 0 : months;
            }
        }

        public bool IsCapableLeader()
        {
            if (Rank >= MilitiaRank.ViceSquadLeader) return true;
            if (Type == MilitiaType.Regular && MonthsOfService >= 6) return true;
            if (Type == MilitiaType.Mobile && MonthsOfService >= 24) return true;

            return false;
        }
    }
}
