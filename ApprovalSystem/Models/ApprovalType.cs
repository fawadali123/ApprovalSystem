using System;
using System.Collections.Generic;

namespace ApprovalSystem.Models
{
    public partial class ApprovalType
    {
        public ApprovalType()
        {
            Approval = new HashSet<Approval>();
            ApprovalHierarchy = new HashSet<ApprovalHierarchy>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Approval> Approval { get; set; }
        public virtual ICollection<ApprovalHierarchy> ApprovalHierarchy { get; set; }
    }
}
