using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Interfaces.Reward
{
    // Manage reward definitions and awarding logic.
    public interface IRewardService
    {
        Task<Rewardsystem_Domain.Domain.Entities.Reward.Reward> CreateRewardAsync(
            string name,
            string? description,
            Rewardsystem_Domain.Domain.Enums.RewardType type,
            int defaultPoints,
            DateTime? effectiveFrom = null,
            DateTime? effectiveTo = null,
            CancellationToken ct = default);

        Task<Rewardsystem_Domain.Domain.Entities.Reward.Reward> UpdateRewardAsync(
            Guid rewardId,
            string name,
            string? description,
            Rewardsystem_Domain.Domain.Enums.RewardType type,
            CancellationToken ct = default);

        Task<Rewardsystem_Domain.Domain.Entities.Reward.Reward?> GetByIdAsync(Guid rewardId, CancellationToken ct = default);

        Task<IReadOnlyList<Rewardsystem_Domain.Domain.Entities.Reward.Reward>> ListAsync(bool includeInactive = false, CancellationToken ct = default);

        Task AwardRewardAsync(Guid rewardId, Guid userId, int points, string? reference = null, CancellationToken ct = default);
    }
}
