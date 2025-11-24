using System;
using System.Collections.Generic;
using System.Text;
using Rewardsystem_Domain.Domain.Entities.Reward;

namespace RewardSystem_Application.Repositories
{

    // Repository abstraction for audit history of points movement
    public interface IPointsTransactionRepository : IRepository<PointsTransaction>
    {
        // Return all points transactions for a specific user
        Task<IReadOnlyList<PointsTransaction>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
