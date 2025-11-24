using Rewardsystem_Domain.Domain.Entities.Reward;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Repositories
{
    // Repository abstraction for RewardTransaction history
    public interface IRewardTransactionRepository : IRepository<RewardTransaction>
    {
        // Get all reward transactions for a user
        Task<IReadOnlyList<RewardTransaction>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
