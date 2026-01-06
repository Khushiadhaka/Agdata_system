using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewardSystem_Application.Repositories;
using RewardSystem_Infrastructure.Infrastructure.Persistence;
using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Infrastructure.Persistence.Repositories
{
    // EF Core repository for UserProfile entity.
    public sealed class UserProfileRepository : IUserProfileRepository
    {
        private readonly RewardDbContext _db;

        public UserProfileRepository(RewardDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        // Get profile by primary key.
        public async Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.UserProfiles.FindAsync(new object[] { id }, ct);
        }

        // Get all profiles.
        public async Task<IReadOnlyList<UserProfile>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.UserProfiles.AsNoTracking().ToListAsync(ct);
        }

        // Add a new profile.
        public async Task AddAsync(UserProfile entity, CancellationToken ct = default)
        {
            await _db.UserProfiles.AddAsync(entity, ct);
        }

        // Update an existing profile.
        public Task UpdateAsync(UserProfile entity, CancellationToken ct = default)
        {
            _db.UserProfiles.Update(entity);
            return Task.CompletedTask;
        }

        // Delete a profile.
        public Task DeleteAsync(UserProfile entity, CancellationToken ct = default)
        {
            _db.UserProfiles.Remove(entity);
            return Task.CompletedTask;
        }

        // Get profile by UserId.
        public async Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _db.UserProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == userId, ct);
        }

        // Remove a profile entity instance (for symmetry with InMemory repo).
        public Task RemoveAsync(UserProfile profile, CancellationToken ct = default)
        {
            _db.UserProfiles.Remove(profile);
            return Task.CompletedTask;
        }
    }
}
