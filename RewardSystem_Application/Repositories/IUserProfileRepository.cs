using System;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Application.Repositories
{
    // Repository for UserProfile entity
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        // Gets profile by user id
        Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
