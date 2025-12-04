using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Infrastructure.Infrastructure.Repositories.InMemory
{
    // In-memory implementation of IUserRepository for tests / console scenarios.
    public sealed class InMemoryUserRepository : InMemoryRepositoryBase<User>, IUserRepository
    {
        // Get user by email (case-insensitive).
        public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            var normalized = email.Trim().ToLowerInvariant();
            var user = _store.Values.FirstOrDefault(u => u.Email.Value.ToLower() == normalized);
            return Task.FromResult(user);
        }

        // Check if a user exists with given email.
        public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
        {
            var normalized = email.Trim().ToLowerInvariant();
            var exists = _store.Values.Any(u => u.Email.Value.ToLower() == normalized);
            return Task.FromResult(exists);
        }

        // Check if a user exists with given employee id.
        public Task<bool> ExistsByEmployeeIdAsync(string employeeId, CancellationToken ct = default)
        {
            var emp = employeeId.Trim();
            var exists = _store.Values.Any(u => u.EmployeeId.Value == emp);
            return Task.FromResult(exists);
        }

        // Get user by employee id.
        public Task<User?> GetByEmployeeIdAsync(string employeeId, CancellationToken ct = default)
        {
            var emp = employeeId.Trim();
            var user = _store.Values.FirstOrDefault(u => u.EmployeeId.Value == emp);
            return Task.FromResult(user);
        }

        
    }
}
