// Redemption request repository abstraction.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Redemption;

namespace RewardSystem_Application.Repositories
{
    // Redemption request repository abstraction.
    public interface IRedemptionRequestRepository
    {
        Task AddAsync(RedemptionRequest request, CancellationToken ct = default);

        Task UpdateAsync(RedemptionRequest request, CancellationToken ct = default);

        Task<RedemptionRequest?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<IReadOnlyList<RedemptionRequest>> GetByUserIdAsync(
            Guid userId,
            CancellationToken ct = default);

        Task<RedemptionRequest?> GetPendingByUserAndProductAsync(
            Guid userId,
            Guid productId,
            CancellationToken ct = default);
    }
}
