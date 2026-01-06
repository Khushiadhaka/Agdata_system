using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Common;

namespace RewardSystem_Infrastructure.Infrastructure.Repositories.InMemory
{
    // Simple in-memory repository base for tests / console apps.
    public abstract class InMemoryRepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        // In-memory storage (Id -> Entity).
        protected readonly Dictionary<Guid, TEntity> _store = new();

        // Get entity by Id.
        public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            _store.TryGetValue(id, out var entity);
            return Task.FromResult(entity);
        }

        // Get all entities.
        public Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default)
        {
            IReadOnlyList<TEntity> list = _store.Values.ToList();
            return Task.FromResult(list);
        }

        // Add entity to store.
        public Task AddAsync(TEntity entity, CancellationToken ct = default)
        {
            // If Id is empty, create one (helpful for tests).
            if (entity.Id == Guid.Empty)
            {
                // Id has protected setter, but we only care about in-memory usage,
                // so this reflection hack is acceptable for test code.
                typeof(BaseEntity)
                    .GetProperty(nameof(BaseEntity.Id))!
                    .SetValue(entity, Guid.NewGuid());
            }

            _store[entity.Id] = entity;
            return Task.CompletedTask;
        }

        // Update entity in store.
        public Task UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            if (entity == null) return Task.CompletedTask;

            _store[entity.Id] = entity;
            return Task.CompletedTask;
        }

        // Delete entity from store.
        public Task DeleteAsync(TEntity entity, CancellationToken ct = default)
        {
            if (entity == null) return Task.CompletedTask;

            _store.Remove(entity.Id);
            return Task.CompletedTask;
        }
    }
}
