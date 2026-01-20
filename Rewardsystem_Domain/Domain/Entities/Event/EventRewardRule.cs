using System;
using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Entities.Event
{
	// Defines a reward rule associated with an EventDefinition.
	public sealed class EventRewardRule : BaseEntity
	{
		public Guid EventDefinitionId { get; private set; }
		public string Condition { get; private set; } = string.Empty;
		public int Points { get; private set; }
		public bool IsActive { get; private set; }

		private EventRewardRule() { }

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

		public void Deactivate()
		{
			if (!IsActive)
				throw new BusinessRuleException("Reward rule is already inactive.");

			IsActive = false;
			MarkUpdated();
		}

		public void Activate()
		{
			if (IsActive)
				throw new BusinessRuleException("Reward rule is already active.");

			IsActive = true;
			MarkUpdated();
		}
	}
}
