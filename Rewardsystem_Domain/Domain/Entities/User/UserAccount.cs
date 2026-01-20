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

		// Account status (Active / Inactive)
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

		// Add points to balance (earning)
		public void AddPoints(int points)
		{
			if (points <= 0)
				throw new ValidationException("Points must be greater than 0.");

			if (Status != AccountStatus.Active)
				throw new BusinessRuleException("Only active accounts can receive points.");

			Points += points;
			MarkUpdated();
		}

		// Deduct points from balance (redeem)
		public void DeductPoints(int points)
		{
			if (points <= 0)
				throw new ValidationException("Points must be greater than 0.");

			if (Status != AccountStatus.Active)
				throw new BusinessRuleException("Only active accounts can redeem points.");

			if (Points < points)
				throw new BusinessRuleException("Insufficient points.");

			Points -= points;
			MarkUpdated();
		}

		// ✅ SAFE ADJUST (earn + / redeem -)
		public void AdjustPoints(int delta)
		{
			if (Status != AccountStatus.Active)
				throw new BusinessRuleException("Account must be active.");

			var newBalance = Points + delta;

			if (newBalance < 0)
				throw new BusinessRuleException("Points balance cannot be negative.");

			Points = newBalance;
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
