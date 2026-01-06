// Handles user account (points) operations and records point transactions.
using System;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Common;
using RewardSystem_Application.Interfaces.Users;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_Application.Services
{
    // Handles user account (points) operations and records point transactions.
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _accountRepo;
        private readonly IPointsTransactionRepository _pointsRepo;
        private readonly IUnitOfWork _uow;

        public UserAccountService(IUserAccountRepository accountRepo, IPointsTransactionRepository pointsRepo, IUnitOfWork uow)
        {
            _accountRepo = accountRepo ?? throw new ArgumentNullException(nameof(accountRepo));
            _pointsRepo = pointsRepo ?? throw new ArgumentNullException(nameof(pointsRepo));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        // Return the current points balance for a user.
        public async Task<int> GetBalanceAsync(Guid userId, CancellationToken ct = default)
        {
            if (userId == Guid.Empty) throw new ValidationException("UserId required.");
            var account = await _accountRepo.GetByUserIdAsync(userId, ct)
                          ?? throw new InvalidOperationException("User account not found.");
            return account.Points;
        }

        // Add points to a user's account and create a points transaction.
        public async Task AddPointsAsync(Guid userId, int points, string? reference = null, CancellationToken ct = default)
        {
            if (userId == Guid.Empty) throw new ValidationException("UserId required.");
            if (points <= 0) throw new ValidationException("Points must be greater than zero.");

            var account = await _accountRepo.GetByUserIdAsync(userId, ct)
                          ?? throw new InvalidOperationException("User account not found.");
            if (account.Status != AccountStatus.Active) throw new BusinessRuleException("Only active accounts can receive points.");

            account.AddPoints(points);
            await _accountRepo.UpdateAsync(account, ct);

            var tx = new PointsTransaction(userId, points, PointsTransactionType.Earn, reference);
            await _pointsRepo.AddAsync(tx, ct);

            await _uow.SaveChangesAsync(ct);
        }

        // Try to deduct points; returns true when deduction was successful.
        public async Task<bool> TryDeductPointsAsync(Guid userId, int points, string? reference = null, CancellationToken ct = default)
        {
            if (userId == Guid.Empty) throw new ValidationException("UserId required.");
            if (points <= 0) throw new ValidationException("Points must be greater than zero.");

            var account = await _accountRepo.GetByUserIdAsync(userId, ct)
                          ?? throw new InvalidOperationException("User account not found.");
            if (account.Points < points) return false;

            account.DeductPoints(points);
            await _accountRepo.UpdateAsync(account, ct);

            var tx = new PointsTransaction(userId, points, PointsTransactionType.Redeem, reference);
            await _pointsRepo.AddAsync(tx, ct);

            await _uow.SaveChangesAsync(ct);
            return true;
        }

        // Set points directly on account (admin operation).
        public async Task SetPointsAsync(Guid userId, int points, string? reference = null, CancellationToken ct = default)
        {
            if (userId == Guid.Empty) throw new ValidationException("UserId required.");
            if (points < 0) throw new ValidationException("Points cannot be negative.");

            var account = await _accountRepo.GetByUserIdAsync(userId, ct)
                          ?? throw new InvalidOperationException("User account not found.");
            account.SetPoints(points);
            await _accountRepo.UpdateAsync(account, ct);

            var tx = new PointsTransaction(userId, points, PointsTransactionType.Adjust, reference);
            await _pointsRepo.AddAsync(tx, ct);

            await _uow.SaveChangesAsync(ct);
        }

        // Return the UserAccount aggregate for advanced scenarios.
        public async Task<Rewardsystem_Domain.Domain.Entities.User.UserAccount?> GetAccountAsync(
            Guid userId,
            CancellationToken ct = default)
        {
            if (userId == Guid.Empty) throw new ValidationException("UserId required.");
            return await _accountRepo.GetByUserIdAsync(userId, ct);
        }
    }
}
