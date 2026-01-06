using Microsoft.EntityFrameworkCore;
using RewardSystem_Application.Repositories;
using RewardSystem_Infrastructure.Infrastructure.Persistence;
using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // EF Core repository for UserAccount (points + password hash).
    public sealed class UserAccountRepository : IUserAccountRepository
    {
        private readonly RewardDbContext _db;

        public UserAccountRepository(RewardDbContext db)
        {
            _db = db;
        }

        // Add a new UserAccount
        public async Task AddAsync(UserAccount account, CancellationToken ct = default)
        {
            await _db.UserAccounts.AddAsync(account, ct);
        }

        // Update an existing UserAccount
        public Task UpdateAsync(UserAccount account, CancellationToken ct = default)
        {
            _db.UserAccounts.Update(account);
            return Task.CompletedTask;
        }

        // Get account by associated UserId
        public Task<UserAccount?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return _db.UserAccounts
                      .AsNoTracking()
                      .FirstOrDefaultAsync(x => x.UserId == userId, ct);
        }
    }
}
