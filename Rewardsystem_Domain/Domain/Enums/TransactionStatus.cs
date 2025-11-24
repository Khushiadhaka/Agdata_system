using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Enums
{
    // Processing status of a transaction
    public enum TransactionStatus
    {
        Pending = 0,
        Completed = 1,
        Failed = 2
    }
}
