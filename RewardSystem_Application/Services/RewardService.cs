// Manages reward definitions and awarding points to users (creates reward + reward points entries).
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
using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_Application.Services
{
    // Manages reward definitions and awarding points to users.
    public class RewardService : IRewardService
    {
        private readonly IRewardRepository _rewardRepo;
        private readonly IRewardPointsRepository _rewardPointsRepo;
        private readonly IUserAccountRepository _accountRepo;
        private readonly IRewardTransactionRepository _rewardTxRepo;
        private readonly IPointsTransactionRepository _pointsTxRepo;
        private readonly IUnitOfWork _uow;

        public RewardService(
            IRewardRepository rewardRepo,
            IRewardPointsRepository rewardPointsRepo,
            IUserAccountRepository accountRepo,
            IRewardTransactionRepository rewardTxRepo,
            IPointsTransactionRepository pointsTxRepo,
            IUnitOfWork uow)
        {
            _rewardRepo = rewardRepo ?? throw new ArgumentNullException(nameof(rewardRepo));
            _rewardPointsRepo = rewardPointsRepo ?? throw new ArgumentNullException(nameof(rewardPointsRepo));
            _accountRepo = accountRepo ?? throw new ArgumentNullException(nameof(accountRepo));
            _rewardTxRepo = rewardTxRepo ?? throw new ArgumentNullException(nameof(rewardTxRepo));
            _pointsTxRepo = pointsTxRepo ?? throw new ArgumentNullException(nameof(pointsTxRepo));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        // Create a new reward with an initial RewardPoints entry.
        public async Task<Reward> CreateRewardAsync(
            string name,
            string? description,
            RewardType type,
            int defaultPoints,
            DateTime? effectiveFrom = null,
            DateTime? effectiveTo = null,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Name required.");
            if (defaultPoints <= 0)
                throw new ValidationException("Default points must be positive.");

            var reward = new Reward(name.Trim(), description?.Trim(), type);
            await _rewardRepo.AddAsync(reward, ct);

            var rp = new RewardPoints(reward.Id, defaultPoints, effectiveFrom, effectiveTo);
            await _rewardPointsRepo.AddAsync(rp, ct);

            await _uow.SaveChangesAsync(ct);
            return reward;
        }

        // Update reward metadata.
        public async Task<Reward> UpdateRewardAsync(
            Guid rewardId,
            string name,
            string? description,
            RewardType type,
            CancellationToken ct = default)
        {
            var r = await _rewardRepo.GetByIdAsync(rewardId, ct)
                    ?? throw new InvalidOperationException("Reward not found.");

            r.Update(name.Trim(), description?.Trim(), type);
            await _rewardRepo.UpdateAsync(r, ct);
            await _uow.SaveChangesAsync(ct);
            return r;
        }

        // Get reward by id.
        public async Task<Reward?> GetByIdAsync(Guid rewardId, CancellationToken ct = default)
        {
            if (rewardId == Guid.Empty) return null;
            return await _rewardRepo.GetByIdAsync(rewardId, ct);
        }

        // List rewards.
        public async Task<IReadOnlyList<Reward>> ListAsync(
            bool includeInactive = false,
            CancellationToken ct = default)
        {
            var list = await _rewardRepo.GetAllAsync(ct);
            return !includeInactive
                ? list.Where(x => x.IsActive).ToList()
                : list;
        }

        // Award reward points to a user and record transactions.
        public async Task AwardRewardAsync(
            Guid rewardId,
            Guid userId,
            int points,
            string? reference = null,
            CancellationToken ct = default)
        {
            if (rewardId == Guid.Empty) throw new ValidationException("RewardId required.");
            if (userId == Guid.Empty) throw new ValidationException("UserId required.");
            if (points <= 0) throw new ValidationException("Points must be positive.");

            var reward = await _rewardRepo.GetByIdAsync(rewardId, ct)
                         ?? throw new InvalidOperationException("Reward not found.");
            if (!reward.IsActive)
                throw new BusinessRuleException("Reward inactive.");

            var acc = await _accountRepo.GetByUserIdAsync(userId, ct)
                      ?? throw new InvalidOperationException("User account not found.");
            if (acc.Status != AccountStatus.Active)
                throw new BusinessRuleException("Account inactive.");

            acc.AddPoints(points);
            await _accountRepo.UpdateAsync(acc, ct);

            var rewardTx = new RewardTransaction(
                rewardId,
                userId,
                points,
                TransactionType.Credit,
                reference);

            await _rewardTxRepo.AddAsync(rewardTx, ct);

            var pointsTx = new PointsTransaction(
                userId,
                points,
                PointsTransactionType.Earn,
                reference ?? $"Reward:{rewardId}");

            await _pointsTxRepo.AddAsync(pointsTx, ct);

            await _uow.SaveChangesAsync(ct);
        }
    }
}
