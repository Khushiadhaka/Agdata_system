using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Transactions;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<IReadOnlyList<Transaction>> GetByTypeAsync(
            TransactionType type,
            CancellationToken cancellationToken = default);
    }
}
