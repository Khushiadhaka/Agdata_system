using System;
using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Entities.Event
{
    // Represents a reusable definition of an event type
    public sealed class EventDefinition : BaseEntity
    {
        // Name of the event definition
        public string Name { get; private set; } = string.Empty;

        // Description of the event definition
        public string Description { get; private set; } = string.Empty;

        // Default reward points for this definition
        public int RewardPoints { get; private set; }

        // Indicates whether the definition is active
        public bool IsActive { get; private set; }

        // Parameterless constructor for EF
        private EventDefinition() { }

        // Creates a new event definition
        public EventDefinition(string name, string description, int rewardPoints)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Event definition name cannot be empty.");

            if (rewardPoints <= 0)
                throw new ValidationException("Reward points must be greater than zero.");

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            RewardPoints = rewardPoints;
            IsActive = true;
        }

        // Updates definition
        public void Update(string name, string description, int rewardPoints)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Event definition name cannot be empty.");

            if (rewardPoints <= 0)
                throw new ValidationException("Reward points must be greater than zero.");

            if (!IsActive)
                throw new BusinessRuleException("Cannot update an inactive event definition.");

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            RewardPoints = rewardPoints;

            MarkUpdated();
        }

        // Deactivates the event definition
        public void Deactivate()
        {
            if (!IsActive)
                throw new BusinessRuleException("Event definition is already inactive.");

            IsActive = false;
            MarkUpdated();
        }

        // Reactivates the event definition
        public void Activate()
        {
            if (IsActive)
                throw new BusinessRuleException("Event definition is already active.");

            IsActive = true;
            MarkUpdated();
        }
    }
}
