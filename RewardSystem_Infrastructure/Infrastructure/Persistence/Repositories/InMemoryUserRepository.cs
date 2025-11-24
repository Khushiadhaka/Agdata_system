using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // In-memory implementation of IUserRepository
    public sealed class InMemoryUserRepository : IUserRepository
    {
        // Internal storage for users
        private readonly List<User> _users = new();

        // Get user by id
        public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }

        // Get all users
        public Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<User> result = _users.ToList();
            return Task.FromResult(result);
        }

        // Add new user
        public Task AddAsync(User entity, CancellationToken cancellationToken = default)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _users.Add(entity);
            return Task.CompletedTask;
        }

        // Update existing user
        public void Update(User entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            var index = _users.FindIndex(u => u.Id == entity.Id);
            if (index >= 0)
                _users[index] = entity;
        }

        // Remove user
        public void Remove(User entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _users.RemoveAll(u => u.Id == entity.Id);
        }

        // Get user by email
        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var normalized = email?.Trim().ToLowerInvariant();
            var user = _users.FirstOrDefault(u => u.Email == normalized);
            return Task.FromResult(user);
        }

        // Extra helper: Get user by employee id
        public Task<User?> GetByEmployeeIdAsync(string employeeId, CancellationToken cancellationToken = default)
        {
            var norm = employeeId?.Trim();
            var user = _users.FirstOrDefault(u => u.EmployeeId == norm);
            return Task.FromResult(user);
        }
    }
}
