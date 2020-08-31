using System;
using System.Collections.Generic;

namespace ApprovalSystem.Models
{
    public partial class ApprovalStatus
    {
        public ApprovalStatus()
        {
            Approval = new HashSet<Approval>();
            ApprovalDetail = new HashSet<ApprovalDetail>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Approval> Approval { get; set; }
        public virtual ICollection<ApprovalDetail> ApprovalDetail { get; set; }
    }
}
