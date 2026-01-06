using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Interfaces.Transaction
{
    // Manage business transactions that may generate reward points.
    public interface ITransactionService
    {
        Task<Rewardsystem_Domain.Domain.Entities.Transactions.Transaction> CreateAsync(
            Guid userId,
            Guid? productId,
            decimal amount,
            int rewardPointsEarned,
            Rewardsystem_Domain.Domain.Enums.TransactionType type,
            CancellationToken ct = default);

        Task<Rewardsystem_Domain.Domain.Entities.Transactions.Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<IReadOnlyList<Rewardsystem_Domain.Domain.Entities.Transactions.Transaction>> ListByUserAsync(Guid userId, CancellationToken ct = default);
    }
}
