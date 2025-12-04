using RewardSystem_Application.Interfaces.Redemption;
using RewardSystem_Application.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Services
{
    // Read-only queries for fulfilled redemption records.
    public class RedemptionRecordService : IRedemptionRecordService
    {
        private readonly IRedemptionRecordRepository _repo;

        public RedemptionRecordService(IRedemptionRecordRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        // Get a redemption record by id.
        public async Task<Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRecord?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty) return null;
            return await _repo.GetByIdAsync(id, ct);
        }

        // List redemption records by user id.
        public async Task<IReadOnlyList<Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRecord>> ListByUserAsync(Guid userId, CancellationToken ct = default)
        {
            var list = await _repo.GetByUserIdAsync(userId, ct);
            return list.ToList();
        }
    }
}
