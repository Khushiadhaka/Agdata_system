using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // In-memory implementation of IEventDefinitionRepository
    public sealed class InMemoryEventDefinitionRepository : IEventDefinitionRepository
    {
        // Internal storage for event definitions
        private readonly List<EventDefinition> _definitions = new();

        // Get definition by id
        public Task<EventDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var def = _definitions.FirstOrDefault(d => d.Id == id);
            return Task.FromResult(def);
        }

        // Get all definitions
        public Task<IReadOnlyList<EventDefinition>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<EventDefinition> result = _definitions.ToList();
            return Task.FromResult(result);
        }

        // Add new definition
        public Task AddAsync(EventDefinition entity, CancellationToken cancellationToken = default)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _definitions.Add(entity);
            return Task.CompletedTask;
        }

        // Update definition
        public void Update(EventDefinition entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            var index = _definitions.FindIndex(d => d.Id == entity.Id);
            if (index >= 0)
                _definitions[index] = entity;
        }

        // Remove definition
        public void Remove(EventDefinition entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _definitions.RemoveAll(d => d.Id == entity.Id);
        }

        // Get definition by name
        public Task<EventDefinition?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var norm = name?.Trim();
            var def = _definitions.FirstOrDefault(d => d.Name == norm);
            return Task.FromResult(def);
        }

        // Get all active definitions
        public Task<IReadOnlyList<EventDefinition>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<EventDefinition> result = _definitions.Where(d => d.IsActive).ToList();
            return Task.FromResult(result);
        }
    }
}
