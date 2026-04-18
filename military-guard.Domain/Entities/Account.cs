using military_guard.Domain.Entities.Common;
using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Domain.Entities
{
    public class Account : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public SystemRole Role { get; set; }
        public bool IsBanned { get; set; }

        public Guid? MilitiaId { get; set; }

        public virtual Militia? Militia { get; set; }
    }
}
