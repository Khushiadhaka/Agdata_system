using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Interfaces.Reward
{
    // Manage reward transaction records.
    public interface IRewardTransactionService
    {
        Task<Rewardsystem_Domain.Domain.Entities.Reward.RewardTransaction> CreateAsync(
            Guid rewardId,
            Guid userId,
            int pointsGranted,
            Rewardsystem_Domain.Domain.Enums.TransactionType transactionType,
            string? reference = null,
            Guid? eventInstanceId = null,
            Guid? redemptionRequestId = null,
            CancellationToken ct = default);

        Task<IReadOnlyList<Rewardsystem_Domain.Domain.Entities.Reward.RewardTransaction>> ListByUserAsync(Guid userId, CancellationToken ct = default);
    }
}
