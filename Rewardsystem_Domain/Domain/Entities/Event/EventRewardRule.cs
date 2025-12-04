using System;
using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Entities.Event
{
    // Defines a reward rule associated with an EventDefinition (e.g., top 3 get X points).
    public sealed class EventRewardRule : BaseEntity
    {
        // Identifier of the related EventDefinition.
        public Guid EventDefinitionId { get; private set; }

        // Condition text describing when the rule applies (non-nullable).
        public string Condition { get; private set; } = string.Empty;

        // Points to award when condition is met.
        public int Points { get; private set; }

        // Whether the rule is active.
        public bool IsActive { get; private set; } = true;

        // Parameterless constructor for EF Core.
        private EventRewardRule() { }

        // Main constructor with validation.
        public EventRewardRule(Guid eventDefinitionId, string condition, int points)
        {
            if (eventDefinitionId == Guid.Empty)
                throw new ValidationException("EventDefinitionId cannot be empty.");

            if (string.IsNullOrWhiteSpace(condition))
                throw new ValidationException("Condition cannot be empty.");

            if (points <= 0)
                throw new ValidationException("Points must be greater than zero.");

            EventDefinitionId = eventDefinitionId;
            Condition = condition.Trim();
            Points = points;
            IsActive = true;
        }

        // Update rule details.
        public void Update(string condition, int points)
        {
            if (!IsActive)
                throw new BusinessRuleException("Cannot update an inactive reward rule.");

            if (string.IsNullOrWhiteSpace(condition))
                throw new ValidationException("Condition cannot be empty.");

            if (points <= 0)
                throw new ValidationException("Points must be greater than zero.");

            Condition = condition.Trim();
            Points = points;
            MarkUpdated();
        }

        // Deactivate the rule.
        public void Deactivate()
        {
            if (!IsActive)
                throw new BusinessRuleException("Reward rule is already inactive.");

            IsActive = false;
            MarkUpdated();
        }

        // Reactivate the rule.
        public void Activate()
        {
            if (IsActive)
                throw new BusinessRuleException("Reward rule is already active.");

            IsActive = true;
            MarkUpdated();
        }
    }
}
