using System;
using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Entities.Event
{
	// Reusable definition of an event type (template).
	public sealed class EventDefinition : BaseEntity
	{
		public string Name { get; private set; } = string.Empty;
		public string Description { get; private set; } = string.Empty;
		public int RewardPoints { get; private set; }
		public bool IsActive { get; private set; }

		private EventDefinition() { }

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

		public void Deactivate()
		{
			if (!IsActive)
				throw new BusinessRuleException("Event definition is already inactive.");

			IsActive = false;
			MarkUpdated();
		}

		public void Activate()
		{
			if (IsActive)
				throw new BusinessRuleException("Event definition is already active.");

			IsActive = true;
			MarkUpdated();
		}
	}
}
