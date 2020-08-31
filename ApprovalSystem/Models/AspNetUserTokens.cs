using System;
using System.Collections.Generic;

namespace ApprovalSystem.Models
{
    public partial class AspNetUserTokens
    {
        public long UserId { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
