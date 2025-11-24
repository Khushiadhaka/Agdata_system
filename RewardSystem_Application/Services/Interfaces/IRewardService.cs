using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Services.Interfaces
{
    // Contract for reward configuration use-cases
    public interface IRewardService
    {
        // Create a new reward program / rule
        Task<Reward> CreateRewardAsync(
            string name,
            string description,
            RewardType type,
            CancellationToken cancellationToken = default);

        // Get a single reward by id
        Task<Reward?> GetRewardByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        // Get all rewards
        Task<IReadOnlyList<Reward>> GetAllRewardsAsync(
            CancellationToken cancellationToken = default);

        // Update an existing reward
        Task UpdateRewardAsync(
            Guid id,
            string name,
            string description,
            RewardType type,
            CancellationToken cancellationToken = default);

        // Deactivate a reward
        Task DeactivateRewardAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        // Create or update reward points configuration
        Task<RewardPoints> ConfigureRewardPointsAsync(
            Guid rewardId,
            int points,
            DateTime? effectiveFrom,
            DateTime? effectiveTo,
            CancellationToken cancellationToken = default);
    }
}

