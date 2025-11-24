using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Reward;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // In-memory implementation of IRewardTransactionRepository
    public sealed class InMemoryRewardTransactionRepository : IRewardTransactionRepository
    {
        // Backing store for RewardTransaction entities
        private readonly List<RewardTransaction> _items = new();

        // Add a new RewardTransaction
        public Task AddAsync(RewardTransaction entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _items.Add(entity);
            return Task.CompletedTask;
        }

        // Get RewardTransaction by Id
        public Task<RewardTransaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var match = _items.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(match);
        }

        // Get all RewardTransactions
        public Task<IReadOnlyList<RewardTransaction>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<RewardTransaction> snapshot = _items.ToList();
            return Task.FromResult(snapshot);
        }

        // Update existing RewardTransaction
        public void Update(RewardTransaction entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var idx = _items.FindIndex(x => x.Id == entity.Id);
            if (idx >= 0)
            {
                _items[idx] = entity;
            }
        }

        // Remove a RewardTransaction
        public void Remove(RewardTransaction entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _items.RemoveAll(x => x.Id == entity.Id);
        }

        // Get all RewardTransactions for a specific user
        public Task<IReadOnlyList<RewardTransaction>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var result = _items
                .Where(t => t.UserId == userId)
                .ToList()
                .AsReadOnly();

            return Task.FromResult((IReadOnlyList<RewardTransaction>)result);
        }
    }

}
