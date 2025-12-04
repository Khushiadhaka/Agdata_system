// Redemption record repository abstraction.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Redemption;

namespace RewardSystem_Application.Repositories
{
    // Redemption record repository abstraction.
    public interface IRedemptionRecordRepository
    {
        Task AddAsync(RedemptionRecord record, CancellationToken ct = default);

        Task<RedemptionRecord?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<IReadOnlyList<RedemptionRecord>> GetByUserIdAsync(
            Guid userId,
            CancellationToken ct = default);
    }
}

