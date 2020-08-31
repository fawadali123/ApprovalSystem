using System;
using System.Collections.Generic;

namespace ApprovalSystem.Models
{
    public partial class ApprovalHierarchyDetail
    {
        public long Id { get; set; }
        public long? ApprovalHierarchyId { get; set; }
        public long? UserId { get; set; }
        public int? Sequence { get; set; }

        public virtual ApprovalHierarchy ApprovalHierarchy { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
