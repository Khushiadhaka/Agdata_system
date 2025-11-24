using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Repositories
{
    public interface IRedemptionRequestRepository : IRepository<RedemptionRequest>
    {
        Task<IReadOnlyList<RedemptionRequest>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<RedemptionRequest>> GetByStatusAsync(
            RedemptionStatus status,
            CancellationToken cancellationToken = default);
    }
}
