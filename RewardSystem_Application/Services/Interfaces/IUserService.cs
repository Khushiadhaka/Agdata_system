using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_Application.Services.Interfaces
{
    // Application service for user operations
    public interface IUserService
    {
        Task<User> CreateUserAsync(
            string name,
            string email,
            string employeeId,
            UserRole role,
            CancellationToken cancellationToken = default);

        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default);

        Task UpdateUserAsync(
            Guid id,
            string name,
            string email,
            UserRole role,
            CancellationToken cancellationToken = default);

        Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);

        Task<UserAccount> CreateUserAccountAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
