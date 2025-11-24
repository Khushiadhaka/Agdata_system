using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Enums
{
    // Status of a redemption request
    public enum RedemptionStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Completed = 3,
        Cancelled = 4
    }
}
