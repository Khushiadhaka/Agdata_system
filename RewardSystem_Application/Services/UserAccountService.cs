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

		public UserAccountService(
			IUserAccountRepository accountRepo,
			IPointsTransactionRepository pointsRepo,
			IUnitOfWork uow)
		{
			_accountRepo = accountRepo;
			_pointsRepo = pointsRepo;
			_uow = uow;
		}

		public async Task<int> GetBalanceAsync(Guid userId, CancellationToken ct = default)
		{
			if (userId == Guid.Empty)
				throw new ValidationException("UserId required.");

			var account = await _accountRepo.GetByUserIdAsync(userId, ct)
						  ?? throw new InvalidOperationException("User account not found.");

			return account.Points;
		}

		public async Task AddPointsAsync(Guid userId, int points, string? reference = null, CancellationToken ct = default)
		{
			if (points <= 0)
				throw new ValidationException("Points must be greater than zero.");

			var account = await _accountRepo.GetByUserIdAsync(userId, ct)
						  ?? throw new InvalidOperationException("User account not found.");

			account.AddPoints(points);
			await _accountRepo.UpdateAsync(account, ct);

			var tx = new PointsTransaction(userId, points, PointsTransactionType.Earn, reference);
			await _pointsRepo.AddAsync(tx, ct);

			await _uow.SaveChangesAsync(ct);
		}

		public async Task<bool> TryDeductPointsAsync(Guid userId, int points, string? reference = null, CancellationToken ct = default)
		{
			if (points <= 0)
				throw new ValidationException("Points must be greater than zero.");

			var account = await _accountRepo.GetByUserIdAsync(userId, ct)
						  ?? throw new InvalidOperationException("User account not found.");

			if (account.Points < points)
				return false;

			account.DeductPoints(points);
			await _accountRepo.UpdateAsync(account, ct);

			var tx = new PointsTransaction(userId, points, PointsTransactionType.Redeem, reference);
			await _pointsRepo.AddAsync(tx, ct);

			await _uow.SaveChangesAsync(ct);
			return true;
		}

		// ✅ SAFE ADMIN ADJUST (absolute -> delta)
		public async Task AdjustPointsAsync(Guid userId, int newPoints, string? reference = null, CancellationToken ct = default)
		{
			if (newPoints < 0)
				throw new ValidationException("Points cannot be negative.");

			var account = await _accountRepo.GetByUserIdAsync(userId, ct)
						  ?? throw new InvalidOperationException("User account not found.");

			var delta = newPoints - account.Points;
			account.AdjustPoints(delta);

			await _accountRepo.UpdateAsync(account, ct);

			var tx = new PointsTransaction(userId, delta, PointsTransactionType.Adjust, reference);
			await _pointsRepo.AddAsync(tx, ct);

			await _uow.SaveChangesAsync(ct);
		}

		public async Task<Rewardsystem_Domain.Domain.Entities.User.UserAccount?> GetAccountAsync(
			Guid userId,
			CancellationToken ct = default)
		{
			if (userId == Guid.Empty)
				throw new ValidationException("UserId required.");

			return await _accountRepo.GetByUserIdAsync(userId, ct);
		}
	}
}
