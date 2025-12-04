using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Reward;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_Infrastructure.Infrastructure.Repositories.InMemory
{
    // In-memory implementation of IRewardRepository.
    public sealed class InMemoryRewardRepository
        : InMemoryRepositoryBase<Reward>, IRewardRepository
    {
        // Get only active rewards.
        public Task<IReadOnlyList<Reward>> GetActiveAsync(CancellationToken ct = default)
        {
            var list = _store.Values.Where(r => r.IsActive).ToList();
            return Task.FromResult<IReadOnlyList<Reward>>(list);
        }
    }

    // In-memory implementation of IRewardPointsRepository.
    public sealed class InMemoryRewardPointsRepository
        : InMemoryRepositoryBase<RewardPoints>, IRewardPointsRepository
    {
        // Get all points rows for a reward.
        public Task<IReadOnlyList<RewardPoints>> GetByRewardIdAsync(Guid rewardId, CancellationToken ct = default)
        {
            var list = _store.Values.Where(p => p.RewardId == rewardId).ToList();
            return Task.FromResult<IReadOnlyList<RewardPoints>>(list);
        }
    }

    // In-memory implementation of IRewardTransactionRepository.
    public sealed class InMemoryRewardTransactionRepository
        : InMemoryRepositoryBase<RewardTransaction>, IRewardTransactionRepository
    {
        // Get reward transactions for user.
        public Task<IReadOnlyList<RewardTransaction>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            var list = _store.Values.Where(t => t.UserId == userId).ToList();
            return Task.FromResult<IReadOnlyList<RewardTransaction>>(list);
        }
    }

    // In-memory implementation of IPointsTransactionRepository.
    public sealed class InMemoryPointsTransactionRepository
        : InMemoryRepositoryBase<PointsTransaction>, IPointsTransactionRepository
    {
        // Get points transactions for user.
        public Task<IReadOnlyList<PointsTransaction>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            var list = _store.Values.Where(t => t.UserId == userId).ToList();
            return Task.FromResult<IReadOnlyList<PointsTransaction>>(list);
        }
    }
}

