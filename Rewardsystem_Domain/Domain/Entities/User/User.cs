using System;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Enums;
using Rewardsystem_Domain.Domain.ValueObjects;

namespace Rewardsystem_Domain.Domain.Entities.User
{
	// Represents a system user (employee).
	public sealed class User : BaseEntity
	{
		// Full display name of the user (non-nullable, default empty string).
		public string Name { get; private set; } = string.Empty;

		// Email value object (initialized by constructor or EF).
		public Email Email { get; private set; } = null!;

		// EmployeeId value object (initialized by constructor or EF).
		public EmployeeId EmployeeId { get; private set; } = null!;

		// Role of the user (Admin / User).
		public UserRole Role { get; private set; }

		// Soft delete flag.
		public bool IsDeleted { get; private set; }

		// Navigation: one-to-one user account (may be null until attached).
		public UserAccount? Account { get; private set; }

		// Navigation: one-to-one user profile (may be null until attached).
		public UserProfile? Profile { get; private set; }

		// Parameterless constructor for EF Core materialization.
		private User() { }

		// Primary constructor for creating new users with validation.
		public User(string name, string email, string employeeId, UserRole role = UserRole.User)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ValidationException("Name cannot be empty.");

			if (string.IsNullOrWhiteSpace(email))
				throw new ValidationException("Email cannot be empty.");

			if (string.IsNullOrWhiteSpace(employeeId))
				throw new ValidationException("EmployeeId cannot be empty.");

			Name = name.Trim();
			Email = new Email(email);
			EmployeeId = new EmployeeId(employeeId);
			Role = role;
			IsDeleted = false;
		}

		// Update user's basic details with validation.
		public void Update(string name, string email, UserRole role)
		{
			if (IsDeleted)
				throw new BusinessRuleException("Cannot update a deleted user.");

			if (string.IsNullOrWhiteSpace(name))
				throw new ValidationException("Name cannot be empty.");

			if (string.IsNullOrWhiteSpace(email))
				throw new ValidationException("Email cannot be empty.");

			Name = name.Trim();
			Email = new Email(email);
			Role = role;

			MarkUpdated();
		}

		// Soft delete the user.
		public void Delete()
		{
			if (IsDeleted)
				throw new BusinessRuleException("User already deleted.");

			IsDeleted = true;
			MarkUpdated();
		}

		// Attach a profile to this user.
		public void AttachProfile(UserProfile profile)
		{
			if (profile is null)
				throw new ValidationException("Profile cannot be null.");

			if (profile.UserId != Id)
				throw new BusinessRuleException("Profile does not belong to this user.");

			Profile = profile;
			MarkUpdated();
		}

		// Attach an account to this user.
		public void AttachAccount(UserAccount account)
		{
			if (account is null)
				throw new ValidationException("Account cannot be null.");

			if (account.UserId != Id)
				throw new BusinessRuleException("Account does not belong to this user.");

			Account = account;
			MarkUpdated();
		}

		// ✅ CENTRAL ACTIVE CHECK (IMPORTANT)
		public bool IsActive()
		{
			return !IsDeleted
				   && Account != null
				   && Account.Status == AccountStatus.Active;
		}
	}
}
