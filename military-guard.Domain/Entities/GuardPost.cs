using military_guard.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Domain.Entities
{
    public class GuardPost : BaseEntity
    {
        public string Name { get; set; } = string.Empty; 
        public string Location { get; set; } = string.Empty;

        // Cấu hình linh hoạt số người cho từng chốt
        public int MinPersonnel { get; set; } = 2;
        public int MaxPersonnel { get; set; } = 4;

        public bool IsActive { get; set; } = true;

        public ICollection<ShiftAssignment> ShiftAssignments { get; set; } = new List<ShiftAssignment>();
    }
}
