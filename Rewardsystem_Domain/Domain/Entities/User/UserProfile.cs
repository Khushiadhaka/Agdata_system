using Rewardsystem_Domain.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Entities.User
{
    // Stores additional profile information for a user
    public sealed class UserProfile : BaseEntity
    {
        // Identifier of the user
        public Guid UserId { get; private set; }

        // Phone number of the user
        public string PhoneNumber { get; private set; } = string.Empty;

        // Department of the user
        public string Department { get; private set; } = string.Empty;

        // Location of the user
        public string Location { get; private set; } = string.Empty;

        // Navigation property to user
        public User? User { get; private set; }

        // Parameterless constructor for EF
        private UserProfile() { }

        // Creates a new user profile
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

        // Updates profile details
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
