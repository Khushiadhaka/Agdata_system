// Reward transaction repository abstraction.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Reward;

namespace RewardSystem_Application.Repositories
{
    // Reward transaction repository abstraction.
    public interface IRewardTransactionRepository
    {
        // Add reward transaction.
        Task AddAsync(RewardTransaction transaction, CancellationToken ct = default);

        // Get reward transactions by user id.
        Task<IReadOnlyList<RewardTransaction>> GetByUserIdAsync(
            Guid userId,
            CancellationToken ct = default);
    }
}
