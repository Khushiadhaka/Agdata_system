// Business transaction repository abstraction.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.Transactions;

namespace RewardSystem_Application.Repositories
{
    // Business transaction repository abstraction.
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction, CancellationToken ct = default);

        Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<IReadOnlyList<Transaction>> GetByUserIdAsync(
            Guid userId,
            CancellationToken ct = default);
    }
}
