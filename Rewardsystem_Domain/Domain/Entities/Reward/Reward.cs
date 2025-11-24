using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Entities.Reward
{
    // Represents a reward program or rule
    public sealed class Reward : BaseEntity
    {
        // Name of the reward
        public string Name { get; private set; } = string.Empty;

        // Description of the reward
        public string Description { get; private set; } = string.Empty;

        // Type of reward
        public RewardType Type { get; private set; }

        // Indicates whether the reward is active
        public bool IsActive { get; private set; }

        // Parameterless constructor for EF
        private Reward() { }

        // Creates a new reward
        public Reward(string name, string description, RewardType type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Reward name cannot be empty.");

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            Type = type;
            IsActive = true;
        }

        // Updates reward details
        public void Update(string name, string description, RewardType type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Reward name cannot be empty.");

            if (!IsActive)
                throw new BusinessRuleException("Cannot update an inactive reward.");

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            Type = type;

            MarkUpdated();
        }

        // Deactivates the reward
        public void Deactivate()
        {
            if (!IsActive)
                throw new BusinessRuleException("Reward is already inactive.");

            IsActive = false;
            MarkUpdated();
        }

        // Reactivates the reward
        public void Activate()
        {
            if (IsActive)
                throw new BusinessRuleException("Reward is already active.");

            IsActive = true;
            MarkUpdated();
        }
    }
    
}
