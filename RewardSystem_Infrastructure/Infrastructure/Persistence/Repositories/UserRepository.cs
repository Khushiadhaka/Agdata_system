using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewardSystem_Application.Repositories;
using RewardSystem_Infrastructure.Infrastructure.Persistence;
using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Repositories
{
    // EF Core repository for User entity.
    public sealed class UserRepository : IUserRepository
    {
        private readonly RewardDbContext _db;

        public UserRepository(RewardDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        // Get user by Id.
        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Users
                .Include(u => u.Account)
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        // Get all users.
        public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default)
        {
            var list = await _db.Users
                .Include(u => u.Account)
                .Include(u => u.Profile)
                .AsNoTracking()
                .ToListAsync(ct);

            return list;
        }

        // Add new user.
        public async Task AddAsync(User entity, CancellationToken ct = default)
        {
            await _db.Users.AddAsync(entity, ct);
        }

        // Update existing user.
        public Task UpdateAsync(User entity, CancellationToken ct = default)
        {
            _db.Users.Update(entity);
            return Task.CompletedTask;
        }

        // Delete user.
        public Task DeleteAsync(User entity, CancellationToken ct = default)
        {
            _db.Users.Remove(entity);
            return Task.CompletedTask;
        }

        // ---------- IUserRepository specific methods ----------

        // Get user by email (case-insensitive).
        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            var normalized = email.Trim().ToLowerInvariant();

            return await _db.Users
                .Include(u => u.Account)
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(
                    u => u.Email.Value.ToLower() == normalized,
                    ct);
        }

        // Check if email already exists.
        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
        {
            var normalized = email.Trim().ToLowerInvariant();
            return await _db.Users
                .AnyAsync(u => u.Email.Value.ToLower() == normalized, ct);
        }

        // Check if employee id already exists.
        public async Task<bool> ExistsByEmployeeIdAsync(string employeeId, CancellationToken ct = default)
        {
            var emp = employeeId.Trim();
            return await _db.Users
                .AnyAsync(u => u.EmployeeId.Value == emp, ct);
        }

        // Get user by employee id.
        public async Task<User?> GetByEmployeeIdAsync(string employeeId, CancellationToken ct = default)
        {
            var emp = employeeId.Trim();
            return await _db.Users
                .Include(u => u.Account)
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.EmployeeId.Value == emp, ct);
        }
    }
}
