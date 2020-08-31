using System;
using System.Collections.Generic;

namespace ApprovalSystem.Models
{
    public partial class Approval
    {
        public Approval()
        {
            ApprovalDetail = new HashSet<ApprovalDetail>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long? ApprovalTypeId { get; set; }
        public long? ApprovalStatusId { get; set; }
        public long? AssignedTo { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime UpdateOn { get; set; }
        public long? UpdateBy { get; set; }
        public bool? IsDeleted { get; set; }
        public string FilePath { get; set; }
        public string CommentsHistory { get; set; }
        public string LastComment { get; set; }

        public virtual ApprovalStatus ApprovalStatus { get; set; }
        public virtual ApprovalType ApprovalType { get; set; }
        public virtual AspNetUsers AssignedToNavigation { get; set; }
        public virtual AspNetUsers CreatedByNavigation { get; set; }
        public virtual AspNetUsers UpdateByNavigation { get; set; }
        public virtual ICollection<ApprovalDetail> ApprovalDetail { get; set; }
    }
}
