using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // In-memory implementation of IUserProfileRepository
    public sealed class InMemoryUserProfileRepository : IUserProfileRepository
    {
        // Internal storage for profiles
        private readonly List<UserProfile> _profiles = new();

        // Get profile by id
        public Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var profile = _profiles.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(profile);
        }

        // Get all profiles
        public Task<IReadOnlyList<UserProfile>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<UserProfile> result = _profiles.ToList();
            return Task.FromResult(result);
        }

        // Add profile
        public Task AddAsync(UserProfile entity, CancellationToken cancellationToken = default)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _profiles.Add(entity);
            return Task.CompletedTask;
        }

        // Update profile
        public void Update(UserProfile entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            var index = _profiles.FindIndex(p => p.Id == entity.Id);
            if (index >= 0)
                _profiles[index] = entity;
        }

        // Remove profile
        public void Remove(UserProfile entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _profiles.RemoveAll(p => p.Id == entity.Id);
        }

        // Get profile by user id
        public Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var profile = _profiles.FirstOrDefault(p => p.UserId == userId);
            return Task.FromResult(profile);
        }
    }
}
