using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // In-memory implementation of IUserAccountRepository
    public sealed class InMemoryUserAccountRepository : IUserAccountRepository
    {
        // Internal storage for accounts
        private readonly List<UserAccount> _accounts = new();

        // Get account by id
        public Task<UserAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == id);
            return Task.FromResult(account);
        }

        // Get all accounts
        public Task<IReadOnlyList<UserAccount>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<UserAccount> result = _accounts.ToList();
            return Task.FromResult(result);
        }

        // Add new account
        public Task AddAsync(UserAccount entity, CancellationToken cancellationToken = default)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _accounts.Add(entity);
            return Task.CompletedTask;
        }

        // Update account
        public void Update(UserAccount entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            var index = _accounts.FindIndex(a => a.Id == entity.Id);
            if (index >= 0)
                _accounts[index] = entity;
        }

        // Remove account
        public void Remove(UserAccount entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _accounts.RemoveAll(a => a.Id == entity.Id);
        }

        // Get account by user id
        public Task<UserAccount?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var account = _accounts.FirstOrDefault(a => a.UserId == userId);
            return Task.FromResult(account);
        }
    }
}
