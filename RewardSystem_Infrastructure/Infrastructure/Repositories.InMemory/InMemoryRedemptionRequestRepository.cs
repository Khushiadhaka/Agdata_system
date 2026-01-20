using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_Infrastructure.Infrastructure.Repositories.InMemory
{
	public sealed class InMemoryRedemptionRequestRepository
		: InMemoryRepositoryBase<RedemptionRequest>,
		  IRedemptionRequestRepository
	{
		public Task<bool> ExistsPendingAsync(
			Guid userId,
			Guid productId,
			CancellationToken ct = default)
		{
			var exists = _store.Values.Any(x =>
				x.UserId == userId &&
				x.ProductId == productId &&
				x.Status == RedemptionStatus.Pending);

			return Task.FromResult(exists);
		}

		public Task<IReadOnlyList<RedemptionRequest>> ListByUserAsync(
			Guid userId,
			CancellationToken ct = default)
		{
			var list = _store.Values
				.Where(x => x.UserId == userId)
				.ToList();

			return Task.FromResult<IReadOnlyList<RedemptionRequest>>(list);
		}
	}
}
