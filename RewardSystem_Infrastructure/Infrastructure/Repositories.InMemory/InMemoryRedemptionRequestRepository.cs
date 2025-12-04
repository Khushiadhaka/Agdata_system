using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_Infrastructure.Infrastructure.Repositories.InMemory
{
    // In-memory implementation of IRedemptionRequestRepository.
    public sealed class InMemoryRedemptionRequestRepository
        : InMemoryRepositoryBase<RedemptionRequest>, IRedemptionRequestRepository
    {
        // Get pending request for user + product.
        public Task<RedemptionRequest?> GetPendingByUserAndProductAsync(Guid userId, Guid productId, CancellationToken ct = default)
        {
            var req = _store.Values.FirstOrDefault(r =>
                r.UserId == userId &&
                r.ProductId == productId &&
                r.Status == RedemptionStatus.Pending);

            return Task.FromResult(req);
        }

        // Get requests by user id.
        public Task<IReadOnlyList<RedemptionRequest>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            var list = _store.Values.Where(r => r.UserId == userId).ToList();
            return Task.FromResult<IReadOnlyList<RedemptionRequest>>(list);
        }
    }

    // In-memory implementation of IRedemptionRecordRepository.
    public sealed class InMemoryRedemptionRecordRepository
        : InMemoryRepositoryBase<RedemptionRecord>, IRedemptionRecordRepository
    {
        // Get records by user id.
        public Task<IReadOnlyList<RedemptionRecord>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            var list = _store.Values.Where(r => r.UserId == userId).ToList();
            return Task.FromResult<IReadOnlyList<RedemptionRecord>>(list);
        }
    }

    // In-memory implementation of IRedemptionProcessRepository.
    public sealed class InMemoryRedemptionProcessRepository
        : InMemoryRepositoryBase<RedemptionProcess>, IRedemptionProcessRepository
    {
        // Get process entries by redemption id.
        public Task<IReadOnlyList<RedemptionProcess>> GetByRedemptionIdAsync(Guid redemptionId, CancellationToken ct = default)
        {
            var list = _store.Values.Where(p => p.RedemptionId == redemptionId).ToList();
            return Task.FromResult<IReadOnlyList<RedemptionProcess>>(list);
        }
    }
}

