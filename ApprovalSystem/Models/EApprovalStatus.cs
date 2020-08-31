using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApprovalSystem.Models
{
    public enum EApprovalStatus
    {
        Pending = 1,
        Approve = 2,
        Reject = 3,
        OnHold = 4,
        InProcess = 5
    }
}
