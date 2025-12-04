using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.User;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_Infrastructure.Infrastructure.Repositories.InMemory
{
    // In-memory implementation of IUserAccountRepository.
    public sealed class InMemoryUserAccountRepository
        : InMemoryRepositoryBase<UserAccount>, IUserAccountRepository
    {
        // Extra store for credentials (UserId → PasswordHash).
        private readonly ConcurrentDictionary<Guid, string> _credentials = new();

        // Get account by user id.
        public Task<UserAccount?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            var acc = _store.Values.FirstOrDefault(a => a.UserId == userId);
            return Task.FromResult(acc);
        }

        // Check if account exists for user.
        public Task<bool> ExistsForUserAsync(Guid userId, CancellationToken ct = default)
        {
            var exists = _store.Values.Any(a => a.UserId == userId);
            return Task.FromResult(exists);
        }

        // Store credentials (password hash) for a user.
        public Task AddCredentialsAsync(Guid userId, string passwordHash, CancellationToken ct = default)
        {
            _credentials[userId] = passwordHash;
            return Task.CompletedTask;
        }

        // Get stored credentials for user.
        public Task<AccountCredentials?> GetCredentialsByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            if (_credentials.TryGetValue(userId, out var hash))
            {
                var creds = new AccountCredentials(userId, hash);
                return Task.FromResult<AccountCredentials?>(creds);
            }

            return Task.FromResult<AccountCredentials?>(null);
        }
    }
}

