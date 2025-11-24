using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Entities.User
{
    // Represents a user in the system
    public sealed class User : BaseEntity
    {
        // Display name of the user
        public string Name { get; private set; } = string.Empty;

        // Email address of the user
        public string Email { get; private set; } = string.Empty;

        // Employee identifier
        public string EmployeeId { get; private set; } = string.Empty;

        // Role assigned to the user
        public UserRole Role { get; private set; }

        // Navigation to user account
        public UserAccount? Account { get; private set; }

        // Soft delete flag
        public bool IsDeleted { get; private set; }

        // Parameterless constructor for EF
        private User() { }

        // Creates a new user with validation
        public User(string name, string email, string employeeId, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ValidationException("Email cannot be empty.");

            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ValidationException("EmployeeId cannot be empty.");

            Name = name.Trim();
            Email = email.Trim().ToLowerInvariant();
            EmployeeId = employeeId.Trim();
            Role = role;
            IsDeleted = false;
        }

        // Updates user basic details
        public void Update(string name, string email, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ValidationException("Email cannot be empty.");

            if (IsDeleted)
                throw new BusinessRuleException("Deleted user cannot be updated.");

            Name = name.Trim();
            Email = email.Trim().ToLowerInvariant();
            Role = role;

            MarkUpdated();
        }

        // Attaches a user account to this user
        public void AttachAccount(UserAccount account)
        {
            if (account == null)
                throw new ValidationException("Account cannot be null.");

            if (account.UserId != Id)
                throw new BusinessRuleException("Account user id must match user id.");

            if (Account != null)
                throw new BusinessRuleException("User already has an account.");

            Account = account;
            MarkUpdated();
        }

        // Marks the user as deleted
        public void Delete()
        {
            if (IsDeleted)
                throw new BusinessRuleException("User is already deleted.");

            IsDeleted = true;
            MarkUpdated();
        }
    }
}
