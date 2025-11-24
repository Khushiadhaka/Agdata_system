using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Reward;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // In-memory implementation of IRewardPointsRepository
    public sealed class InMemoryRewardPointsRepository : IRewardPointsRepository
    {
        // Backing store for RewardPoints entities
        private readonly List<RewardPoints> _items = new();

        // Add a new RewardPoints entity
        public Task AddAsync(RewardPoints entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _items.Add(entity);
            return Task.CompletedTask;
        }

        // Get RewardPoints by primary Id
        public Task<RewardPoints?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var match = _items.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(match);
        }

        // Get all RewardPoints entries
        public Task<IReadOnlyList<RewardPoints>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<RewardPoints> snapshot = _items.ToList();
            return Task.FromResult(snapshot);
        }

        // Update an existing RewardPoints entity
        public void Update(RewardPoints entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var idx = _items.FindIndex(x => x.Id == entity.Id);
            if (idx >= 0)
            {
                _items[idx] = entity;
            }
        }

        // Remove a RewardPoints entity
        public void Remove(RewardPoints entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _items.RemoveAll(x => x.Id == entity.Id);
        }

        // Get RewardPoints row for a given RewardId
        public Task<RewardPoints?> GetByRewardIdAsync(
            Guid rewardId,
            CancellationToken cancellationToken = default)
        {
            if (rewardId == Guid.Empty)
                throw new ArgumentException("RewardId cannot be empty.", nameof(rewardId));

            var match = _items.FirstOrDefault(rp => rp.RewardId == rewardId);
            return Task.FromResult(match);
        }
    }
}
