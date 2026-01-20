using System;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_Application.Interfaces.Users
{
	// Service for operations on the user's account (points balance).
	public interface IUserAccountService
	{
		// Get current points balance for a user.
		Task<int> GetBalanceAsync(Guid userId, CancellationToken ct = default);

		// Add points to a user's account (admin or event awarding).
		Task AddPointsAsync(
			Guid userId,
			int points,
			string? reference = null,
			CancellationToken ct = default);

		// Try to deduct points from a user's account (returns true on success).
		Task<bool> TryDeductPointsAsync(
			Guid userId,
			int points,
			string? reference = null,
			CancellationToken ct = default);

		// ADMIN SAFE ADJUST (absolute set replaced with delta-based adjust)
		Task AdjustPointsAsync(
			Guid userId,
			int newPoints,
			string? reference = null,
			CancellationToken ct = default);

		// Get the UserAccount aggregate for advanced scenarios.
		Task<Rewardsystem_Domain.Domain.Entities.User.UserAccount?> GetAccountAsync(
			Guid userId,
			CancellationToken ct = default);
	}
}
