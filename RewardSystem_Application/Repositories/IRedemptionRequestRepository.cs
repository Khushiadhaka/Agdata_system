using Rewardsystem_Domain.Domain.Entities.Redemption;

namespace RewardSystem_Application.Repositories
{
	public interface IRedemptionRequestRepository
		: IRepository<RedemptionRequest>
	{
		Task<bool> ExistsPendingAsync(
			Guid userId,
			Guid productId,
			CancellationToken ct = default);

		Task<IReadOnlyList<RedemptionRequest>> ListByUserAsync(
			Guid userId,
			CancellationToken ct = default);
	}
}
