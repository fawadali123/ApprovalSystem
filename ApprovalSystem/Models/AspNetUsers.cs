﻿using System;
using System.Collections.Generic;

namespace ApprovalSystem.Models
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            ApprovalAssignedToNavigation = new HashSet<Approval>();
            ApprovalCreatedByNavigation = new HashSet<Approval>();
            ApprovalDetail = new HashSet<ApprovalDetail>();
            ApprovalHierarchyDetail = new HashSet<ApprovalHierarchyDetail>();
            ApprovalUpdateByNavigation = new HashSet<Approval>();
            AspNetUserClaims = new HashSet<AspNetUserClaims>();
            AspNetUserLogins = new HashSet<AspNetUserLogins>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
            AspNetUserTokens = new HashSet<AspNetUserTokens>();
        }

        public long Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public virtual ICollection<Approval> ApprovalAssignedToNavigation { get; set; }
        public virtual ICollection<Approval> ApprovalCreatedByNavigation { get; set; }
        public virtual ICollection<ApprovalDetail> ApprovalDetail { get; set; }
        public virtual ICollection<ApprovalHierarchyDetail> ApprovalHierarchyDetail { get; set; }
        public virtual ICollection<Approval> ApprovalUpdateByNavigation { get; set; }
        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
    }
}