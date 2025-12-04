using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Interfaces.Redemption
{
    // Manage fulfillment records and completing requests.
    public interface IRedemptionProcessService
    {
        Task<Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRecord> CreateRecordAsync(
            Guid userId,
            Guid productId,
            string? reference = null,
            CancellationToken ct = default);

        Task<Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRecord> CompleteRequestAsync(
            Guid requestId,
            string? reference = null,
            CancellationToken ct = default);
    }
}
