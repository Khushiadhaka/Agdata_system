// Reward repository abstraction.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Reward;

namespace RewardSystem_Application.Repositories
{
    // Reward repository abstraction.
    public interface IRewardRepository
    {
        // Get reward by id.
        Task<Reward?> GetByIdAsync(Guid id, CancellationToken ct = default);

        // Get all rewards.
        Task<IReadOnlyList<Reward>> GetAllAsync(CancellationToken ct = default);

        // Add reward.
        Task AddAsync(Reward reward, CancellationToken ct = default);

        // Update reward.
        Task UpdateAsync(Reward reward, CancellationToken ct = default);
    }
}
