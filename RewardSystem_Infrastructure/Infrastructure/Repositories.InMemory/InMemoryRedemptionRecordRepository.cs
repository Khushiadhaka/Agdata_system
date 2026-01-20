using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Redemption;

namespace RewardSystem_Infrastructure.Infrastructure.Repositories.InMemory
{
	public sealed class InMemoryRedemptionRecordRepository
		: InMemoryRepositoryBase<RedemptionRecord>,
		  IRedemptionRecordRepository
	{
		public Task<IReadOnlyList<RedemptionRecord>> ListByUserAsync(
			Guid userId,
			CancellationToken ct = default)
		{
			var list = _store.Values
				.Where(x => x.UserId == userId)
				.ToList();

			return Task.FromResult<IReadOnlyList<RedemptionRecord>>(list);
		}
	}
}
