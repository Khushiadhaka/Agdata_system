// Manages business transactions and optionally awards points based on transaction rules.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Common;
using RewardSystem_Application.Interfaces.Transaction;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Entities.Transactions;
using Rewardsystem_Domain.Domain.Enums;
using Rewardsystem_Domain.Domain.Exceptions;

namespace RewardSystem_Application.Services
{
    // Manages business transactions and optionally awards points based on transaction rules.
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _txRepo;
        private readonly IUserAccountRepository _accountRepo;
        private readonly IPointsTransactionRepository _pointsRepo;
        private readonly IUnitOfWork _uow;

        public TransactionService(
            ITransactionRepository txRepo,
            IUserAccountRepository accountRepo,
            IPointsTransactionRepository pointsRepo,
            IUnitOfWork uow)
        {
            _txRepo = txRepo ?? throw new ArgumentNullException(nameof(txRepo));
            _accountRepo = accountRepo ?? throw new ArgumentNullException(nameof(accountRepo));
            _pointsRepo = pointsRepo ?? throw new ArgumentNullException(nameof(pointsRepo));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        // Create a transaction and award points if configured.
        public async Task<Transaction> CreateAsync(
            Guid userId,
            Guid? productId,
            decimal amount,
            int rewardPointsEarned,
            TransactionType type,
            CancellationToken ct = default)
        {
            if (userId == Guid.Empty) throw new ValidationException("UserId required.");
            if (amount <= 0) throw new ValidationException("Amount must be positive.");
            if (rewardPointsEarned < 0) throw new ValidationException("Reward points cannot be negative.");

            var tx = new Transaction(userId, productId, amount, rewardPointsEarned, type);
            await _txRepo.AddAsync(tx, ct);

            if (rewardPointsEarned > 0)
            {
                var acc = await _accountRepo.GetByUserIdAsync(userId, ct)
                          ?? throw new InvalidOperationException("Account not found.");

                if (acc.Status != AccountStatus.Active)
                    throw new BusinessRuleException("Account not active.");

                acc.AddPoints(rewardPointsEarned);
                await _accountRepo.UpdateAsync(acc, ct);

                var ptsTx = new PointsTransaction(
                    userId,
                    rewardPointsEarned,
                    PointsTransactionType.Earn,
                    $"Transaction:{tx.Id}");

                await _pointsRepo.AddAsync(ptsTx, ct);
            }

            await _uow.SaveChangesAsync(ct);
            return tx;
        }

        // Get transaction by id.
        public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty) return null;
            return await _txRepo.GetByIdAsync(id, ct);
        }

        // List transactions for a user.
        public async Task<IReadOnlyList<Transaction>> ListByUserAsync(Guid userId, CancellationToken ct = default)
        {
            var list = await _txRepo.GetByUserIdAsync(userId, ct);
            return list.ToList();
        }
    }
}
