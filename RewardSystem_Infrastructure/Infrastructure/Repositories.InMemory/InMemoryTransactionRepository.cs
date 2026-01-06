using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_Infrastructure.Infrastructure.Repositories.InMemory
{
    // In-memory implementation of ITransactionRepository.
    public sealed class InMemoryTransactionRepository
        : InMemoryRepositoryBase<Transaction>, ITransactionRepository
    {
        // Get transactions by user id.
        public Task<IReadOnlyList<Transaction>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            var list = _store.Values.Where(t => t.UserId == userId).ToList();
            return Task.FromResult<IReadOnlyList<Transaction>>(list);
        }
    }
}

