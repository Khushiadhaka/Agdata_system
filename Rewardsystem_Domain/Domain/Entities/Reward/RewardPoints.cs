using Rewardsystem_Domain.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Entities.Reward
{
    // Defines point configuration for a reward
    public sealed class RewardPoints : BaseEntity
    {
        // Identifier of the reward
        public Guid RewardId { get; private set; }

        // Points associated with this reward
        public int Points { get; private set; }

        // Optional effective-from date
        public DateTime? EffectiveFrom { get; private set; }

        // Optional effective-to date
        public DateTime? EffectiveTo { get; private set; }

        // Parameterless constructor for EF
        private RewardPoints() { }

        // Creates a new reward points configuration
        public RewardPoints(Guid rewardId, int points, DateTime? effectiveFrom, DateTime? effectiveTo)
        {
            if (rewardId == Guid.Empty)
                throw new ValidationException("RewardId cannot be empty.");

            if (points <= 0)
                throw new ValidationException("Points must be greater than zero.");

            if (effectiveFrom.HasValue && effectiveTo.HasValue && effectiveTo <= effectiveFrom)
                throw new ValidationException("EffectiveTo must be after EffectiveFrom.");

            RewardId = rewardId;
            Points = points;
            EffectiveFrom = effectiveFrom;
            EffectiveTo = effectiveTo;
        }

        // Updates points configuration
        public void UpdatePoints(int points, DateTime? effectiveFrom, DateTime? effectiveTo)
        {
            if (points <= 0)
                throw new ValidationException("Points must be greater than zero.");

            if (effectiveFrom.HasValue && effectiveTo.HasValue && effectiveTo <= effectiveFrom)
                throw new ValidationException("EffectiveTo must be after EffectiveFrom.");

            Points = points;
            EffectiveFrom = effectiveFrom;
            EffectiveTo = effectiveTo;

            MarkUpdated();
        }
    }
}
