using System;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Enums;

namespace Rewardsystem_Domain.Domain.Entities.User
{
    // Stores reward points balance + login credentials for a user.
    public sealed class UserAccount : BaseEntity
    {
        // Associated User ID
        public Guid UserId { get; private set; }

        // Current points balance
        public int Points { get; private set; }

        // Account status (Active/Inactive)
        public AccountStatus Status { get; private set; }

        // Hashed password used for authentication
        public string PasswordHash { get; private set; } = string.Empty;

        // Navigation property to User
        public User? User { get; private set; }

        // Constructor for EF Core
        private UserAccount() { }

        // Main constructor – new account starts with 0 points and Active status
        public UserAccount(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ValidationException("Invalid UserId.");

            UserId = userId;
            Points = 0;
            Status = AccountStatus.Active;
        }

        // Set / change hashed password
        public void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ValidationException("Password hash cannot be empty.");

            PasswordHash = passwordHash;
            MarkUpdated();
        }

        // Add points to balance
        public void AddPoints(int points)
        {
            if (points <= 0)
                throw new ValidationException("Points must be greater than 0.");

            if (Status != AccountStatus.Active)
                throw new BusinessRuleException("Only active accounts can receive points.");

            Points += points;
            MarkUpdated();
        }

        // Deduct points from balance
        public void DeductPoints(int points)
        {
            if (points <= 0)
                throw new ValidationException("Points must be greater than 0.");

            if (Points < points)
                throw new BusinessRuleException("Insufficient points.");

            Points -= points;
            MarkUpdated();
        }

        // Set exact points (admin use)
        public void SetPoints(int points)
        {
            if (points < 0)
                throw new ValidationException("Points cannot be negative.");

            Points = points;
            MarkUpdated();
        }

        // Activate account
        public void Activate()
        {
            Status = AccountStatus.Active;
            MarkUpdated();
        }

        // Deactivate account
        public void Deactivate()
        {
            Status = AccountStatus.Inactive;
            MarkUpdated();
        }
    }
}
