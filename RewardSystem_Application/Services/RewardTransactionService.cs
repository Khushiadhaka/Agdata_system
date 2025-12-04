// Manages creation and listing of reward transactions.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Common;
using RewardSystem_Application.Interfaces.Reward;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Reward;

namespace RewardSystem_Application.Services
{
    // Manages creation and listing of reward transactions.
    public class RewardTransactionService : IRewardTransactionService
    {
        private readonly IRewardTransactionRepository _repo;
        private readonly IUnitOfWork _uow;

        public RewardTransactionService(
            IRewardTransactionRepository repo,
            IUnitOfWork uow)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        // Create a reward transaction record.
        public async Task<RewardTransaction> CreateAsync(
            Guid rewardId,
            Guid userId,
            int pointsGranted,
            Rewardsystem_Domain.Domain.Enums.TransactionType transactionType,
            string? reference = null,
            Guid? eventInstanceId = null,
            Guid? redemptionRequestId = null,
            CancellationToken ct = default)
        {
            if (rewardId == Guid.Empty)
                throw new ArgumentException("rewardId required.", nameof(rewardId));
            if (userId == Guid.Empty)
                throw new ArgumentException("userId required.", nameof(userId));
            if (pointsGranted <= 0)
                throw new ArgumentException("points must be positive.", nameof(pointsGranted));

            var rt = new RewardTransaction(
                rewardId,
                userId,
                pointsGranted,
                transactionType,
                reference,
                eventInstanceId,
                redemptionRequestId);

            await _repo.AddAsync(rt, ct);
            await _uow.SaveChangesAsync(ct);
            return rt;
        }

        // List reward transactions for a user.
        public async Task<IReadOnlyList<RewardTransaction>> ListByUserAsync(
            Guid userId,
            CancellationToken ct = default)
        {
            var list = await _repo.GetByUserIdAsync(userId, ct);
            return list.ToList();
        }
    }
}
