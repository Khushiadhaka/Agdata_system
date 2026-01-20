using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_Infrastructure.Infrastructure.Repositories.InMemory
{
	// ---------------- EVENT DEFINITION ----------------

	// In-memory implementation of IEventDefinitionRepository.
	public sealed class InMemoryEventDefinitionRepository
		: InMemoryRepositoryBase<EventDefinition>, IEventDefinitionRepository
	{
		// Get only active event definitions.
		public Task<IReadOnlyList<EventDefinition>> GetActiveAsync(CancellationToken ct = default)
		{
			ct.ThrowIfCancellationRequested();

			var list = _store.Values
							 .Where(d => d.IsActive)
							 .ToList()
							 .AsReadOnly();

			return Task.FromResult<IReadOnlyList<EventDefinition>>(list);
		}
	}

	// ---------------- EVENT INSTANCE ----------------

	// In-memory implementation of IEventInstanceRepository.
	public sealed class InMemoryEventInstanceRepository
		: InMemoryRepositoryBase<EventInstance>, IEventInstanceRepository
	{
		// Get event instances by definition id.
		public Task<IReadOnlyList<EventInstance>> GetByDefinitionIdAsync(
			Guid defId,
			CancellationToken ct = default)
		{
			if (defId == Guid.Empty)
				throw new ArgumentException("EventDefinitionId cannot be empty.", nameof(defId));

			ct.ThrowIfCancellationRequested();

			var list = _store.Values
							 .Where(i => i.EventDefinitionId == defId)
							 .ToList()
							 .AsReadOnly();

			return Task.FromResult<IReadOnlyList<EventInstance>>(list);
		}
	}

	// ---------------- EVENT REWARD RULE ----------------

	// In-memory implementation of IEventRewardRuleRepository.
	public sealed class InMemoryEventRewardRuleRepository
		: InMemoryRepositoryBase<EventRewardRule>, IEventRewardRuleRepository
	{
		// Get reward rules for an event definition.
		public Task<IReadOnlyList<EventRewardRule>> GetByEventDefinitionIdAsync(
			Guid defId,
			CancellationToken ct = default)
		{
			if (defId == Guid.Empty)
				throw new ArgumentException("EventDefinitionId cannot be empty.", nameof(defId));

			ct.ThrowIfCancellationRequested();

			var list = _store.Values
							 .Where(r => r.EventDefinitionId == defId)
							 .ToList()
							 .AsReadOnly();

			return Task.FromResult<IReadOnlyList<EventRewardRule>>(list);
		}
	}
}
