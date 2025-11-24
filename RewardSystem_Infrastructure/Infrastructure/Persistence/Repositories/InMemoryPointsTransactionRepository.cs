using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Reward;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // In-memory implementation of IPointsTransactionRepository
    public sealed class InMemoryPointsTransactionRepository : IPointsTransactionRepository
    {
        // Backing store in memory
        private readonly List<PointsTransaction> _items = new();

        // Add a new transaction
        public Task AddAsync(PointsTransaction entity, CancellationToken cancellationToken = default)
        {
            _items.Add(entity);
            return Task.CompletedTask;
        }

        // Get by primary key
        public Task<PointsTransaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = _items.FirstOrDefault(t => t.Id == id);
            return Task.FromResult(result);
        }

        // Get all transactions
        public Task<IReadOnlyList<PointsTransaction>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<PointsTransaction> result = _items.ToList();
            return Task.FromResult(result);
        }

        // Update an existing transaction
        public void Update(PointsTransaction entity)
        {
            var index = _items.FindIndex(t => t.Id == entity.Id);
            if (index >= 0)
            {
                _items[index] = entity;
            }
        }

        // Remove a transaction
        public void Remove(PointsTransaction entity)
        {
            _items.RemoveAll(t => t.Id == entity.Id);
        }

        // Get all transactions for a specific user
        public Task<IReadOnlyList<PointsTransaction>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            IReadOnlyList<PointsTransaction> result =
                _items.Where(t => t.UserId == userId).ToList();

            return Task.FromResult(result);
        }
    }

}
