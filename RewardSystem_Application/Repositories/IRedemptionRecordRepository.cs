using Rewardsystem_Domain.Domain.Entities.Redemption;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Repositories
{
    public interface IRedemptionRecordRepository : IRepository<RedemptionRecord>
    {
        Task<IReadOnlyList<RedemptionRecord>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<RedemptionRecord>> GetByProductIdAsync(
            Guid productId,
            CancellationToken cancellationToken = default);
    }
}
