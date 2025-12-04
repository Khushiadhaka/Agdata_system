using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Application.Repositories
{
    // Repository for UserAccount (points + credentials).
    public interface IUserAccountRepository
    {
        // Add new UserAccount entity.
        Task AddAsync(UserAccount account, CancellationToken ct = default);

        // Update existing UserAccount.
        Task UpdateAsync(UserAccount account, CancellationToken ct = default);

        // Get UserAccount by associated UserId.
        Task<UserAccount?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    }
}
