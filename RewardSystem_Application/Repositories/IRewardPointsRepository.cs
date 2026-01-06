// Reward points repository abstraction.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Reward;

namespace RewardSystem_Application.Repositories
{
    // Reward points repository abstraction.
    public interface IRewardPointsRepository
    {
        // Add new points configuration.
        Task AddAsync(RewardPoints entity, CancellationToken ct = default);

        // Get all points configurations for a reward.
        Task<IReadOnlyList<RewardPoints>> GetByRewardIdAsync(
            Guid rewardId,
            CancellationToken ct = default);
    }
}
