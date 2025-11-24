using System;
using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Entities.Event
{
    // Reward rule associated with an event definition
    public sealed class EventRewardRule : BaseEntity
    {
        // Identifier of the event definition
        public Guid EventDefinitionId { get; private set; }

        // Condition description
        public string Condition { get; private set; } = string.Empty;

        // Points to award when condition is met
        public int Points { get; private set; }

        // Indicates whether the rule is active
        public bool IsActive { get; private set; }

        // Parameterless constructor for EF
        private EventRewardRule() { }

        // Creates a new event reward rule
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

        // Updates rule details
        public void Update(string condition, int points)
        {
            if (string.IsNullOrWhiteSpace(condition))
                throw new ValidationException("Condition cannot be empty.");

            if (points <= 0)
                throw new ValidationException("Points must be greater than zero.");

            if (!IsActive)
                throw new BusinessRuleException("Cannot update an inactive reward rule.");

            Condition = condition.Trim();
            Points = points;

            MarkUpdated();
        }

        // Deactivates the rule
        public void Deactivate()
        {
            if (!IsActive)
                throw new BusinessRuleException("Reward rule is already inactive.");

            IsActive = false;
            MarkUpdated();
        }

        // Reactivates the rule
        public void Activate()
        {
            if (IsActive)
                throw new BusinessRuleException("Reward rule is already active.");

            IsActive = true;
            MarkUpdated();
        }
    }
}
