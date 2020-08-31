using System;
using System.Collections.Generic;

namespace ApprovalSystem.Models
{
    public partial class ApprovalHierarchy
    {
        public ApprovalHierarchy()
        {
            ApprovalHierarchyDetail = new HashSet<ApprovalHierarchyDetail>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long ApprovalTypeId { get; set; }
        public bool? IsActive { get; set; }

        public virtual ApprovalType ApprovalType { get; set; }
        public virtual ICollection<ApprovalHierarchyDetail> ApprovalHierarchyDetail { get; set; }
    }
}
