using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Enums
{
    // Kind of transaction
    public enum TransactionType
    {
        Purchase = 0,
        Refund = 1,
        Transfer = 2,
        Bonus = 3
    }
}
