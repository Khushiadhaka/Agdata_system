using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Application.Interfaces.Event
{
	// Manages event definition templates (Admin operations).
	public interface IEventDefinitionService
	{
		Task<EventDefinition> CreateAsync(
			string name,
			string? description,
			int rewardPoints,
			CancellationToken ct = default);

		Task<EventDefinition> UpdateAsync(
			Guid id,
			string name,
			string? description,
			int rewardPoints,
			CancellationToken ct = default);

		Task<EventDefinition?> GetByIdAsync(
			Guid id,
			CancellationToken ct = default);

		// List definitions (optionally include inactive ones).
		Task<IReadOnlyList<EventDefinition>> ListAsync(
			bool includeInactive = false,
			CancellationToken ct = default);

		Task DeactivateAsync(Guid id, CancellationToken ct = default);

		Task ActivateAsync(Guid id, CancellationToken ct = default);
	}
}
