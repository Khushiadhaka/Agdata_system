using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_Infrastructure.Infrastructure.Repositories.InMemory
{
    // In-memory implementation of IEventDefinitionRepository.
    public sealed class InMemoryEventDefinitionRepository
        : InMemoryRepositoryBase<EventDefinition>, IEventDefinitionRepository
    {
        // Get only active definitions.
        public Task<IReadOnlyList<EventDefinition>> GetActiveAsync(CancellationToken ct = default)
        {
            var list = _store.Values.Where(d => d.IsActive).ToList();
            return Task.FromResult<IReadOnlyList<EventDefinition>>(list);
        }
    }

    // In-memory implementation of IEventInstanceRepository.
    public sealed class InMemoryEventInstanceRepository
        : InMemoryRepositoryBase<EventInstance>, IEventInstanceRepository
    {
        // Get instances by definition id.
        public Task<IReadOnlyList<EventInstance>> GetByDefinitionIdAsync(Guid defId, CancellationToken ct = default)
        {
            var list = _store.Values.Where(i => i.EventDefinitionId == defId).ToList();
            return Task.FromResult<IReadOnlyList<EventInstance>>(list);
        }
    }

    // In-memory implementation of IEventRewardRuleRepository.
    public sealed class InMemoryEventRewardRuleRepository
        : InMemoryRepositoryBase<EventRewardRule>, IEventRewardRuleRepository
    {
        // Get rules for a definition.
        public Task<IReadOnlyList<EventRewardRule>> GetByEventDefinitionIdAsync(Guid defId, CancellationToken ct = default)
        {
            var list = _store.Values.Where(r => r.EventDefinitionId == defId).ToList();
            return Task.FromResult<IReadOnlyList<EventRewardRule>>(list);
        }
    }
}

