using System;
using System.Collections.Generic;

namespace ApprovalSystem.Models
{
    public partial class ApprovalDetail
    {
        public long Id { get; set; }
        public long? ApprovalId { get; set; }
        public long? UserId { get; set; }
        public int? Sequence { get; set; }
        public long? ApprovalStatusId { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string Comments { get; set; }

        public virtual Approval Approval { get; set; }
        public virtual ApprovalStatus ApprovalStatus { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
