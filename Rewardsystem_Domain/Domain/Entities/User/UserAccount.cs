using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Entities.User
{
    // Stores reward points for a user
    public sealed class UserAccount : BaseEntity
    {
        // Identifier of the owning user
        public Guid UserId { get; private set; }

        // Current points balance
        public int Points { get; private set; }

        // Account status (for blocking redemptions etc.)
        public AccountStatus Status { get; private set; } = AccountStatus.Active;

        // Navigation back to user
        public User? User { get; private set; }

        // Parameterless constructor for EF
        private UserAccount() { }

        // Creates a new account for a user
        public UserAccount(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ValidationException("Invalid UserId.");

            UserId = userId;
            Points = 0;
        }

        // Adds points to the account
        public void AddPoints(int points)
        {
            if (points <= 0)
                throw new ValidationException("Points must be greater than 0.");

            if (Status != AccountStatus.Active)
                throw new BusinessRuleException("Only active accounts can receive points.");

            Points += points;
            MarkUpdated();
        }

        // Deducts points from the account
        public void DeductPoints(int points)
        {
            if (points <= 0)
                throw new ValidationException("Points must be greater than 0.");

            if (Points < points)
                throw new BusinessRuleException("Insufficient points for deduction.");

            Points -= points;
            MarkUpdated();
        }

        // Sets points directly
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
            if (Status == AccountStatus.Active)
                throw new BusinessRuleException("Account is already active.");

            Status = AccountStatus.Active;
            MarkUpdated();
        }

        // Deactivate account
        public void Deactivate()
        {
            if (Status == AccountStatus.Inactive)
                throw new BusinessRuleException("Account is already inactive.");

            Status = AccountStatus.Inactive;
            MarkUpdated();
        }
    }
}
