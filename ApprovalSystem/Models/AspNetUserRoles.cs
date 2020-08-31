using System;
using System.Collections.Generic;

namespace ApprovalSystem.Models
{
    public partial class AspNetUserRoles
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }

        public virtual AspNetRoles Role { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
