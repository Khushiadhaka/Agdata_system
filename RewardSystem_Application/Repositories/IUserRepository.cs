using Rewardsystem_Domain.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_Application.Repositories
{
    // Specialized repository for User aggregate.
    public interface IUserRepository : IRepository<User>
    {
        // Find user by email.
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);

        // Find user by employee id.
        Task<User?> GetByEmployeeIdAsync(string employeeId, CancellationToken ct = default);

        // Check if user exists by email.
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);

        // Check if user exists by employee id.
        Task<bool> ExistsByEmployeeIdAsync(string employeeId, CancellationToken ct = default);
    }
}
