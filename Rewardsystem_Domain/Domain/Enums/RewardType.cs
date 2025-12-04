using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Enums
{
    // Type of reward configuration
    public enum RewardType
    {
        FixedPoints = 0,
        PercentageOfAmount = 1,
        Tiered = 2,
        Generic = 3
    }
}
