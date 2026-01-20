using Rewardsystem_Domain.Domain.Entities.Redemption;

namespace RewardSystem_Application.Repositories
{
	public interface IRedemptionRecordRepository
		: IRepository<RedemptionRecord>
	{
		Task<IReadOnlyList<RedemptionRecord>> ListByUserAsync(
			Guid userId,
			CancellationToken ct = default);
	}
}
