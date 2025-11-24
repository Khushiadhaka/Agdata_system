using System;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Application.Repositories
{
    // Repository for UserAccount entity
    public interface IUserAccountRepository : IRepository<UserAccount>
    {
        // Gets account by user id
        Task<UserAccount?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
