using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Interfaces.Redemption
{
    // Manage lifecycle of user redemption requests.
    public interface IRedemptionRequestService
    {
        Task<Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRequest> CreateAsync(
            Guid userId,
            Guid productId,
            int pointsUsed,
            CancellationToken ct = default);

        Task<Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRequest> UpdateStatusAsync(
            Guid requestId,
            Rewardsystem_Domain.Domain.Enums.RedemptionStatus newStatus,
            string? note = null,
            CancellationToken ct = default);

        Task<Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRequest?> GetByIdAsync(Guid requestId, CancellationToken ct = default);

        Task<IReadOnlyList<Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRequest>> ListByUserAsync(Guid userId, CancellationToken ct = default);
    }
}
