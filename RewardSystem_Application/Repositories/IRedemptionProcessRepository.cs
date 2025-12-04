// Redemption process repository abstraction.
using System;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Redemption;

namespace RewardSystem_Application.Repositories
{
    // Redemption process repository abstraction.
    public interface IRedemptionProcessRepository
    {
        Task AddAsync(RedemptionProcess process, CancellationToken ct = default);

        Task<RedemptionProcess?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task UpdateAsync(RedemptionProcess process, CancellationToken ct = default);
    }
}
