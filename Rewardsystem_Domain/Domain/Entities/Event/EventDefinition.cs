using System;
using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Entities.Event
{
    // Reusable definition of an event type (template).
    public sealed class EventDefinition : BaseEntity
    {
        // Definition name (non-nullable, default empty).
        public string Name { get; private set; } = string.Empty;

        // Description for the definition (non-nullable, default empty).
        public string Description { get; private set; } = string.Empty;

        // Default reward points associated with this definition.
        public int RewardPoints { get; private set; }

        // Whether this definition is active and can be used.
        public bool IsActive { get; private set; } = true;

        // Parameterless constructor for EF Core.
        private EventDefinition() { }

        // Main constructor with validation.
        public EventDefinition(string name, string? description, int rewardPoints)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Event definition name cannot be empty.");

            if (rewardPoints <= 0)
                throw new ValidationException("Reward points must be greater than zero.");

            Name = name.Trim();
            Description = (description ?? string.Empty).Trim();
            RewardPoints = rewardPoints;
            IsActive = true;
        }

        // Update definition details.
        public void Update(string name, string? description, int rewardPoints)
        {
            if (!IsActive)
                throw new BusinessRuleException("Cannot update an inactive event definition.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Event definition name cannot be empty.");

            if (rewardPoints <= 0)
                throw new ValidationException("Reward points must be greater than zero.");

            Name = name.Trim();
            Description = (description ?? string.Empty).Trim();
            RewardPoints = rewardPoints;
            MarkUpdated();
        }

        // Deactivate definition.
        public void Deactivate()
        {
            if (!IsActive)
                throw new BusinessRuleException("Event definition is already inactive.");

            IsActive = false;
            MarkUpdated();
        }

        // Reactivate definition.
        public void Activate()
        {
            if (IsActive)
                throw new BusinessRuleException("Event definition is already active.");

            IsActive = true;
            MarkUpdated();
        }
    }
}
