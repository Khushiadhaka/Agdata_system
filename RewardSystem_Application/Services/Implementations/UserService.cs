using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services.Interfaces;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_Application.Services.Implementations
{
    // Application service that orchestrates user-related operations
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IUserRepository userRepository,
            IUserAccountRepository userAccountRepository,
            IUserProfileRepository userProfileRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userAccountRepository = userAccountRepository ?? throw new ArgumentNullException(nameof(userAccountRepository));
            _userProfileRepository = userProfileRepository ?? throw new ArgumentNullException(nameof(userProfileRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<User> CreateUserAsync(
            string name,
            string email,
            string employeeId,
            UserRole role,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Name is required.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ValidationException("Email is required.");

            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ValidationException("EmployeeId is required.");

            var existing = await _userRepository.GetByEmailAsync(email.Trim(), cancellationToken);
            if (existing != null)
                throw new BusinessRuleException("User with this email already exists.");

            var user = new User(name, email, employeeId, role);

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user;
        }

        public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ValidationException("Id cannot be empty.");

            return _userRepository.GetByIdAsync(id, cancellationToken);
        }

        public Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _userRepository.GetAllAsync(cancellationToken);
        }

        public async Task UpdateUserAsync(
            Guid id,
            string name,
            string email,
            UserRole role,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ValidationException("Id cannot be empty.");

            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user == null)
                throw new BusinessRuleException("User not found.");

            user.Update(name, email, role);

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ValidationException("Id cannot be empty.");

            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user == null)
                throw new BusinessRuleException("User not found.");

            user.Delete();
            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<UserAccount> CreateUserAccountAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty)
                throw new ValidationException("UserId cannot be empty.");

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new BusinessRuleException("User not found.");

            var existingAccount = await _userAccountRepository.GetByUserIdAsync(userId, cancellationToken);
            if (existingAccount != null)
                throw new BusinessRuleException("User already has an account.");

            var account = new UserAccount(userId);

            await _userAccountRepository.AddAsync(account, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return account;
        }
    }
}
