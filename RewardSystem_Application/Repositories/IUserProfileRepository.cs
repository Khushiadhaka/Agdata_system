using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Application.Repositories
{
    // Repository contract for UserProfile entity.
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        // Get profile by UserId.
        Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);

        // Remove a profile entity instance.
        Task RemoveAsync(UserProfile profile, CancellationToken ct = default);
    }
}
