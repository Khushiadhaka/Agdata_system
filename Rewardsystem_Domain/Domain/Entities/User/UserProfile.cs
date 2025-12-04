using System;
using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Entities.User
{
    // Stores additional profile details for a user (phone, department, location).
    public sealed class UserProfile : BaseEntity
    {
        // Associated User ID.
        public Guid UserId { get; private set; }

        // Phone number (non-nullable).
        public string PhoneNumber { get; private set; } = string.Empty;

        // Department name (non-nullable).
        public string Department { get; private set; } = string.Empty;

        // Location (non-nullable).
        public string Location { get; private set; } = string.Empty;

        // Navigation back to user (optional).
        public User? User { get; private set; }

        // Parameterless constructor for EF Core.
        private UserProfile() { }

        // Primary constructor with validation.
        public UserProfile(Guid userId, string phoneNumber, string department, string location)
        {
            if (userId == Guid.Empty)
                throw new ValidationException("UserId is invalid.");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ValidationException("PhoneNumber cannot be empty.");

            if (string.IsNullOrWhiteSpace(department))
                throw new ValidationException("Department cannot be empty.");

            if (string.IsNullOrWhiteSpace(location))
                throw new ValidationException("Location cannot be empty.");

            UserId = userId;
            PhoneNumber = phoneNumber.Trim();
            Department = department.Trim();
            Location = location.Trim();
        }

        // Update profile details with validation and mark updated timestamp.
        public void Update(string phoneNumber, string department, string location)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ValidationException("PhoneNumber cannot be empty.");

            if (string.IsNullOrWhiteSpace(department))
                throw new ValidationException("Department cannot be empty.");

            if (string.IsNullOrWhiteSpace(location))
                throw new ValidationException("Location cannot be empty.");

            PhoneNumber = phoneNumber.Trim();
            Department = department.Trim();
            Location = location.Trim();

            MarkUpdated();
        }
    }
}
