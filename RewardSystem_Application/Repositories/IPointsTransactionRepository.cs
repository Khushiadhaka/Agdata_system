using System;
using System.Collections.Generic;
using System.Text;
using Rewardsystem_Domain.Domain.Entities.Reward;

namespace RewardSystem_Application.Repositories
{

    public interface IPointsTransactionRepository
    {
        Task AddAsync(PointsTransaction tx, CancellationToken ct = default);

        Task<IReadOnlyList<PointsTransaction>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    }
}
