using Rewardsystem_Domain.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Interfaces.Users
{
    // User profile service contract (used by API layer).
    public interface IUserProfileService
    {
        // Create a new profile or update existing one for a user.
        Task<UserProfile> CreateOrUpdateAsync(
            Guid userId,
            string phoneNumber,
            string department,
            string location,
            CancellationToken ct = default);

        // Get profile by user id (or null if not found).
        Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);

        // Delete profile by user id, returns true if deleted.
        Task<bool> DeleteProfileAsync(Guid userId, CancellationToken ct = default);

    }
}