using military_guard.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Domain.Entities
{
    public class HolidayEvent : BaseEntity
    {
        public string Name { get; set; } = string.Empty; 
        public DateOnly Date { get; set; } 
        public bool RequiresMobileForces { get; set; } = true;
        public string? Description { get; set; }
    }
}
