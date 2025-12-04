using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Common;

namespace RewardSystem_Application.Repositories
{
    // Generic repository contract for all entities.
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        // Get entity by Id (or null if not found).
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);

        // Get all entities.
        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default);

        // Add a new entity.
        Task AddAsync(TEntity entity, CancellationToken ct = default);

        // Update an existing entity.
        Task UpdateAsync(TEntity entity, CancellationToken ct = default);

        // Delete an entity.
        Task DeleteAsync(TEntity entity, CancellationToken ct = default);
    }
}

