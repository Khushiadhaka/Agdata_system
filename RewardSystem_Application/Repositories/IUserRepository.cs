using System;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Application.Repositories
{
    // Repository for User aggregate
    public interface IUserRepository : IRepository<User>
    {
        // Finds user by email (or null)
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
