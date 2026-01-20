using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Application.Interfaces.Event
{
	// Manages reward rules for event definitions (Admin operations).
	public interface IEventRewardRuleService
	{
		Task<EventRewardRule> CreateAsync(
			Guid eventDefinitionId,
			string condition,
			int points,
			CancellationToken ct = default);

		Task<EventRewardRule> UpdateAsync(
			Guid ruleId,
			string condition,
			int points,
			CancellationToken ct = default);

		Task<IReadOnlyList<EventRewardRule>> GetByDefinitionAsync(
			Guid eventDefinitionId,
			CancellationToken ct = default);

		Task DeactivateAsync(Guid ruleId, CancellationToken ct = default);

		Task ActivateAsync(Guid ruleId, CancellationToken ct = default);
	}
}
