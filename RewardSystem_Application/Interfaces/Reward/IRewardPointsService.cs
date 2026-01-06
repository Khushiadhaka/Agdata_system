using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Interfaces.Reward
{
    // Manage reward points configuration versions.
    public interface IRewardPointsService
    {
        Task<Rewardsystem_Domain.Domain.Entities.Reward.RewardPoints> CreateAsync(
            Guid rewardId,
            int points,
            DateTime? effectiveFrom = null,
            DateTime? effectiveTo = null,
            CancellationToken ct = default);

        Task<Rewardsystem_Domain.Domain.Entities.Reward.RewardPoints?> GetLatestForRewardAsync(Guid rewardId, CancellationToken ct = default);
    }
}
