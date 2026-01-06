// Manages reward points versions/configuration for rewards.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Common;
using RewardSystem_Application.Interfaces.Reward;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Reward;

namespace RewardSystem_Application.Services
{
    // Manages reward points versions/configuration for rewards.
    public class RewardPointsService : IRewardPointsService
    {
        private readonly IRewardPointsRepository _repo;
        private readonly IUnitOfWork _uow;

        public RewardPointsService(IRewardPointsRepository repo, IUnitOfWork uow)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        // Create a RewardPoints configuration record.
        public async Task<RewardPoints> CreateAsync(
            Guid rewardId,
            int points,
            DateTime? effectiveFrom = null,
            DateTime? effectiveTo = null,
            CancellationToken ct = default)
        {
            if (rewardId == Guid.Empty)
                throw new ValidationException("RewardId required.");
            if (points <= 0)
                throw new ValidationException("Points must be positive.");

            var rp = new RewardPoints(rewardId, points, effectiveFrom, effectiveTo);
            await _repo.AddAsync(rp, ct);
            await _uow.SaveChangesAsync(ct);
            return rp;
        }

        // Return latest RewardPoints entry for a reward.
        public async Task<RewardPoints?> GetLatestForRewardAsync(
            Guid rewardId,
            CancellationToken ct = default)
        {
            var list = await _repo.GetByRewardIdAsync(rewardId, ct);
            return list.OrderByDescending(x => x.CreatedAt).FirstOrDefault();
        }
    }
}
