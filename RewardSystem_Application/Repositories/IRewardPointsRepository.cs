using Rewardsystem_Domain.Domain.Entities.Reward;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Repositories
{
    // Repository for reward points configuration
    public interface IRewardPointsRepository : IRepository<RewardPoints>
    {
        // Returns current points config for a reward, or null if none exists
        Task<RewardPoints?> GetByRewardIdAsync(
            Guid rewardId,
            CancellationToken cancellationToken = default);
    }
}
