using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.User;
using RewardSystem_Infrastructure.Infrastructure.Repositories.InMemory;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_Infrastructure.Repositories.InMemory
{
    // In-memory implementation of IUserProfileRepository for tests / console apps.
    public sealed class InMemoryUserProfileRepository
        : InMemoryRepositoryBase<UserProfile>, IUserProfileRepository
    {
        // Get profile by UserId.
        public Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            var profile = _store.Values.FirstOrDefault(p => p.UserId == userId);
            return Task.FromResult(profile);
        }

        // Remove profile using base DeleteAsync.
        public Task RemoveAsync(UserProfile profile, CancellationToken ct = default)
        {
            return DeleteAsync(profile, ct);
        }
    }
}
