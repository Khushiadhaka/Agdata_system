using System;
using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Entities.Reward
{
    // Configuration of how many points a Reward grants (or requires).
    public sealed class RewardPoints : BaseEntity
    {
        // Associated Reward identifier.
        public Guid RewardId { get; private set; }

        // Points value for this configuration (positive integer).
        public int Points { get; private set; }

        // Optional effective start date for this configuration.
        public DateTime? EffectiveFrom { get; private set; }

        // Optional effective end date for this configuration.
        public DateTime? EffectiveTo { get; private set; }

        // Parameterless constructor for EF Core.
        private RewardPoints() { }

        // Main constructor with validation.
        public RewardPoints(Guid rewardId, int points, DateTime? effectiveFrom = null, DateTime? effectiveTo = null)
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

        // Update points configuration with validation.
        public void UpdatePoints(int points, DateTime? effectiveFrom = null, DateTime? effectiveTo = null)
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

        // Check whether this configuration is currently active (by date).
        public bool IsEffective(DateTime at)
        {
            if (EffectiveFrom.HasValue && at < EffectiveFrom.Value) return false;
            if (EffectiveTo.HasValue && at > EffectiveTo.Value) return false;
            return true;
        }
    }
}
