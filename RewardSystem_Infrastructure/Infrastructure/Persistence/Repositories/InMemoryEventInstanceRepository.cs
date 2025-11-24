using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    public sealed class InMemoryEventInstanceRepository : IEventInstanceRepository
    {
        private readonly List<EventInstance> _instances = new();

        public Task AddAsync(EventInstance entity, CancellationToken cancellationToken = default)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _instances.Add(entity);
            return Task.CompletedTask;
        }

        public Task<EventInstance?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var item = _instances.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(item);
        }

        public Task<IReadOnlyList<EventInstance>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<EventInstance> result = _instances.ToList();
            return Task.FromResult(result);
        }

        // Update instance
        public void Update(EventInstance entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            var index = _instances.FindIndex(i => i.Id == entity.Id);
            if (index >= 0)
                _instances[index] = entity;
        }

        public Task<IReadOnlyList<EventInstance>> GetByDefinitionIdAsync(
            Guid definitionId,
            CancellationToken cancellationToken = default)
        {
            IReadOnlyList<EventInstance> list = _instances
                .Where(x => x.EventDefinitionId == definitionId)
                .ToList();

            return Task.FromResult(list);
        }

        // Remove instance
        public void Remove(EventInstance entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _instances.RemoveAll(i => i.Id == entity.Id);
        }
    }
}
