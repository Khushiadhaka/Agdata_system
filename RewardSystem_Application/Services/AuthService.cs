using RewardSystem_Application.Common;

using RewardSystem_Application.Interfaces.Security;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Enums;
using Rewardsystem_Domain.Domain.Exceptions;
using System.Security.Authentication;
using RewardSystem_Application.Interfaces.Auth;

namespace RewardSystem_Application.Services
{
    // Authentication service implementing IAuthService (register + login + JWT generation).
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IUserAccountRepository _accountRepo;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtTokenGenerator _jwtGenerator;
        private readonly IUnitOfWork _uow;

        public AuthService(
            IUserRepository userRepo,
            IUserAccountRepository accountRepo,
            IPasswordHasher hasher,
            IJwtTokenGenerator jwtGenerator,
            IUnitOfWork uow)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _accountRepo = accountRepo ?? throw new ArgumentNullException(nameof(accountRepo));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            _jwtGenerator = jwtGenerator ?? throw new ArgumentNullException(nameof(jwtGenerator));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        // Authenticate a user and return JWT token.
        public async Task<string> LoginAsync(string email, string password, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ValidationException("Email is required.");
            if (string.IsNullOrWhiteSpace(password))
                throw new ValidationException("Password is required.");

            var normalized = email.Trim().ToLowerInvariant();
            var user = await _userRepo.GetByEmailAsync(normalized, ct);

            if (user == null || user.IsDeleted)
                throw new InvalidCredentialsException("Invalid email or password.");

            var account = await _accountRepo.GetByUserIdAsync(user.Id, ct);
            if (account == null || string.IsNullOrWhiteSpace(account.PasswordHash))
                throw new InvalidCredentialsException("Invalid email or password.");

            if (!_hasher.Verify(account.PasswordHash, password))
                throw new InvalidCredentialsException("Invalid email or password.");

            var claims = new Dictionary<string, string>
            {
                ["sub"] = user.Id.ToString(),
                ["email"] = user.Email.ToString(),
                ["role"] = user.Role.ToString()
            };

            return _jwtGenerator.GenerateToken(user.Id, user.Email.ToString(), claims);
        }

        // Register a new user, create account and store hashed password, then return JWT token.
        public async Task<string> RegisterAsync(
            string name,
            string email,
            string employeeId,
            string password,
            string role = "User",
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Name required.");
            if (string.IsNullOrWhiteSpace(email))
                throw new ValidationException("Email required.");
            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ValidationException("EmployeeId required.");
            if (string.IsNullOrWhiteSpace(password))
                throw new ValidationException("Password required.");

            var normalized = email.Trim().ToLowerInvariant();

            if (await _userRepo.ExistsByEmailAsync(normalized, ct))
                throw new DuplicateUserException("Email already exists.");

            if (await _userRepo.ExistsByEmployeeIdAsync(employeeId.Trim(), ct))
                throw new DuplicateUserException("EmployeeId already exists.");

            var parsedRole = Enum.TryParse<UserRole>(role ?? "User", true, out var r)
                ? r
                : UserRole.User;

            var user = new User(name.Trim(), normalized, employeeId.Trim(), parsedRole);
            await _userRepo.AddAsync(user, ct);

            // Create account + save password hash
            var account = new UserAccount(user.Id);
            var hashed = _hasher.Hash(password);
            account.SetPasswordHash(hashed);
            await _accountRepo.AddAsync(account, ct);

            await _uow.SaveChangesAsync(ct);

            var claims = new Dictionary<string, string>
            {
                ["sub"] = user.Id.ToString(),
                ["email"] = user.Email.ToString(),
                ["role"] = user.Role.ToString()
            };

            return _jwtGenerator.GenerateToken(user.Id, user.Email.ToString(), claims);
        }
    }
}
