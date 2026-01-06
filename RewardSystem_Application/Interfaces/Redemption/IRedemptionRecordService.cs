using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Interfaces.Redemption
{
    // Read/query fulfilled redemption records.
    public interface IRedemptionRecordService
    {
        Task<Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRecord?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<IReadOnlyList<Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRecord>> ListByUserAsync(Guid userId, CancellationToken ct = default);
    }
}
