using System;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Enums;

namespace Rewardsystem_Domain.Domain.Entities.Reward
{
    // Represents a logical reward program or reward definition.
    public sealed class Reward : BaseEntity
    {
        // Human-friendly name of the reward.
        public string Name { get; private set; } = string.Empty;

        // Long description (optional).
        public string Description { get; private set; } = string.Empty;

        // Type of the reward (for categorization).
        public RewardType Type { get; private set; }

        // Whether the reward is active and can be used.
        public bool IsActive { get; private set; } = true;

        // Navigation: latest points configuration (optional).
        public RewardPoints? LatestPoints { get; private set; }

        // Parameterless constructor for EF Core.
        private Reward() { }

        // Main constructor with validation.
        public Reward(string name, string? description, RewardType type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Reward name cannot be empty.");

            Name = name.Trim();
            Description = (description ?? string.Empty).Trim();
            Type = type;
            IsActive = true;
        }

        // Update reward metadata.
        public void Update(string name, string? description, RewardType type)
        {
            if (!IsActive)
                throw new BusinessRuleException("Cannot update an inactive reward.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Reward name cannot be empty.");

            Name = name.Trim();
            Description = (description ?? string.Empty).Trim();
            Type = type;

            MarkUpdated();
        }

        // Deactivate reward.
        public void Deactivate()
        {
            if (!IsActive)
                throw new BusinessRuleException("Reward is already inactive.");

            IsActive = false;
            MarkUpdated();
        }

        // Reactivate reward.
        public void Activate()
        {
            if (IsActive)
                throw new BusinessRuleException("Reward is already active.");

            IsActive = true;
            MarkUpdated();
        }

        // Attach latest RewardPoints configuration.
        public void AttachPointsConfig(RewardPoints points)
        {
            if (points == null)
                throw new ValidationException("Points configuration cannot be null.");

            if (points.RewardId != Id)
                throw new BusinessRuleException("Points configuration does not belong to this reward.");

            LatestPoints = points;
            MarkUpdated();
        }
    }
}
