using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // In-memory repository for EventRewardRule
    public sealed class InMemoryEventRewardRuleRepository : IEventRewardRuleRepository
    {
        private readonly List<EventRewardRule> _items = new();

        public Task AddAsync(EventRewardRule entity, CancellationToken cancellationToken = default)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _items.Add(entity);
            return Task.CompletedTask;
        }

        public Task<EventRewardRule?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(item);
        }

        public Task<IReadOnlyList<EventRewardRule>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<EventRewardRule> result = _items.ToList();
            return Task.FromResult(result);
        }

        public void Update(EventRewardRule entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            var index = _items.FindIndex(x => x.Id == entity.Id);
            if (index >= 0)
                _items[index] = entity;
        }

        public void Remove(EventRewardRule entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _items.RemoveAll(x => x.Id == entity.Id);
        }

        // Get all rules for a given event definition
        public Task<IReadOnlyList<EventRewardRule>> GetByDefinitionIdAsync(
            Guid eventDefinitionId,
            CancellationToken cancellationToken = default)
        {
            IReadOnlyList<EventRewardRule> result = _items
                .Where(x => x.EventDefinitionId == eventDefinitionId)
                .ToList();

            return Task.FromResult(result);
        }
    }
}
