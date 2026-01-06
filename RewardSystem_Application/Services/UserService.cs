using RewardSystem_Application.Common;
using RewardSystem_Application.Interfaces.Users;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Enums;
using Rewardsystem_Domain.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Services
{
    // Manages user creation, retrieval, update, and soft-delete operations.
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IUserAccountRepository _accountRepo;
        private readonly IUnitOfWork _uow;

        public UserService(IUserRepository userRepo, IUserAccountRepository accountRepo, IUnitOfWork uow)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _accountRepo = accountRepo ?? throw new ArgumentNullException(nameof(accountRepo));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        // Create a new user and initialize their account.
        public async Task<Rewardsystem_Domain.Domain.Entities.User.User> CreateUserAsync(string name, string email, string employeeId, string role, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ValidationException("Name required.");
            if (string.IsNullOrWhiteSpace(email)) throw new ValidationException("Email required.");
            if (string.IsNullOrWhiteSpace(employeeId)) throw new ValidationException("EmployeeId required.");

            var normalized = email.Trim().ToLowerInvariant();
            if (await _userRepo.ExistsByEmailAsync(normalized, ct)) throw new DuplicateUserException("Email exists.");
            if (await _userRepo.ExistsByEmployeeIdAsync(employeeId.Trim(), ct)) throw new DuplicateUserException("EmployeeId exists.");

            var parsedRole = Enum.TryParse<UserRole>(role ?? "User", true, out var r) ? r : UserRole.User;
            var user = new Rewardsystem_Domain.Domain.Entities.User.User(name.Trim(), normalized, employeeId.Trim(), parsedRole);

            await _userRepo.AddAsync(user, ct);
            var account = new Rewardsystem_Domain.Domain.Entities.User.UserAccount(user.Id);
            await _accountRepo.AddAsync(account, ct);

            await _uow.SaveChangesAsync(ct);
            return user;
        }

        // Retrieve a user by id; returns null if missing or deleted.
        public async Task<Rewardsystem_Domain.Domain.Entities.User.User?> GetByIdAsync(Guid userId, CancellationToken ct = default)
        {
            if (userId == Guid.Empty) return null;
            var user = await _userRepo.GetByIdAsync(userId, ct);
            if (user == null || user.IsDeleted) return null;
            return user;
        }

        // Retrieve a user by employee id; returns null if missing or deleted.
        public async Task<Rewardsystem_Domain.Domain.Entities.User.User?> GetByEmployeeIdAsync(string employeeId, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(employeeId)) return null;
            var user = await _userRepo.GetByEmployeeIdAsync(employeeId.Trim(), ct);
            if (user == null || user.IsDeleted) return null;
            return user;
        }

        // List active (non-deleted) users.
        public async Task<IReadOnlyList<Rewardsystem_Domain.Domain.Entities.User.User>> GetAllAsync(CancellationToken ct = default)
        {
            var all = await _userRepo.GetAllAsync(ct);
            return all.Where(u => !u.IsDeleted).ToList();
        }

        // Update user details after validations.
        public async Task<Rewardsystem_Domain.Domain.Entities.User.User> UpdateUserAsync(Guid userId, string name, string email, string role, CancellationToken ct = default)
        {
            if (userId == Guid.Empty) throw new ValidationException("UserId required.");
            if (string.IsNullOrWhiteSpace(name)) throw new ValidationException("Name required.");
            if (string.IsNullOrWhiteSpace(email)) throw new ValidationException("Email required.");

            var user = await _userRepo.GetByIdAsync(userId, ct) ?? throw new InvalidOperationException("User not found.");
            if (user.IsDeleted) throw new BusinessRuleException("Cannot update deleted user.");

            var normalized = email.Trim().ToLowerInvariant();
            if (!string.Equals(user.Email.ToString(), normalized, StringComparison.OrdinalIgnoreCase)
                && await _userRepo.ExistsByEmailAsync(normalized, ct))
            {
                throw new DuplicateUserException("Another user with same email exists.");
            }

            var parsedRole = Enum.TryParse<UserRole>(role ?? user.Role.ToString(), true, out var r) ? r : user.Role;
            user.Update(name.Trim(), normalized, parsedRole);

            await _userRepo.UpdateAsync(user, ct);
            await _uow.SaveChangesAsync(ct);
            return user;
        }

        // Soft-delete a user (marks IsDeleted).
        public async Task<bool> DeleteUserAsync(Guid userId, CancellationToken ct = default)
        {
            if (userId == Guid.Empty) throw new ValidationException("UserId required.");
            var user = await _userRepo.GetByIdAsync(userId, ct) ?? throw new InvalidOperationException("User not found.");
            if (user.IsDeleted) return false;

            // Optional: validate business constraints (pending redemptions, etc.).

            user.Delete();
            await _userRepo.UpdateAsync(user, ct);
            await _uow.SaveChangesAsync(ct);
            return true;
        }
    }
}
