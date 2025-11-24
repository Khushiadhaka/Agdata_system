using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Services.Interfaces
{
    // Coordinates redemption workflow
    public interface IRedemptionService
    {
        // Creates a new redemption request for a product
        Task<RedemptionRequest> CreateRedemptionRequestAsync(
            Guid userId,
            Guid productId,
            int pointsUsed,
            CancellationToken cancellationToken = default);

        // Approves a redemption request
        Task ApproveRedemptionAsync(Guid redemptionRequestId, CancellationToken cancellationToken = default);

        // Rejects a redemption request
        Task RejectRedemptionAsync(Guid redemptionRequestId, CancellationToken cancellationToken = default);

        // Completes (fulfilled) redemption
        Task CompleteRedemptionAsync(Guid redemptionRequestId, CancellationToken cancellationToken = default);

        // Gets user redemption requests by status
        Task<IReadOnlyList<RedemptionRequest>> GetUserRequestsByStatusAsync(
            Guid userId,
            RedemptionStatus status,
            CancellationToken cancellationToken = default);
    }
}
