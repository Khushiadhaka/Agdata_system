using Rewardsystem_Domain.Domain.Entities.Reward;
using System;
using System.Collections.Generic;
using System.Text;
using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Application.Interfaces.Users
{
    // Service for core user management operations (create, read, update, soft-delete).
    public interface IUserService
    {
        // Create and persist a new user; returns the created domain User entity.
        Task<Rewardsystem_Domain.Domain.Entities.User.User> CreateUserAsync(
            string name,
            string email,
            string employeeId,
            string role,
            CancellationToken ct = default);

        // Retrieve a user by its identifier.
        Task<Rewardsystem_Domain.Domain.Entities.User.User?> GetByIdAsync(Guid userId, CancellationToken ct = default);

        // Retrieve a user by employee id.
        Task<Rewardsystem_Domain.Domain.Entities.User.User?> GetByEmployeeIdAsync(string employeeId, CancellationToken ct = default);

        // List all users (usually excluding soft-deleted ones).
        Task<IReadOnlyList<Rewardsystem_Domain.Domain.Entities.User.User>> GetAllAsync(CancellationToken ct = default);

        // Update a user's basic details and return the updated entity.
        Task<Rewardsystem_Domain.Domain.Entities.User.User> UpdateUserAsync(
            Guid userId,
            string name,
            string email,
            string role,
            CancellationToken ct = default);

        // Soft-delete a user (returns true if deletion was performed).
        Task<bool> DeleteUserAsync(Guid userId, CancellationToken ct = default);
    }
}
