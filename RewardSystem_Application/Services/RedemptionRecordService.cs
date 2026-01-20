using RewardSystem_Application.Interfaces.Redemption;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Entities.Redemption;

namespace RewardSystem_Application.Services
{
	public class RedemptionRecordService : IRedemptionRecordService
	{
		private readonly IRedemptionRecordRepository _repo;

		public RedemptionRecordService(IRedemptionRecordRepository repo)
		{
			_repo = repo;
		}

		public Task<RedemptionRecord?> GetByIdAsync(Guid id, CancellationToken ct = default)
		{
			if (id == Guid.Empty) return Task.FromResult<RedemptionRecord?>(null);
			return _repo.GetByIdAsync(id, ct);
		}

		public Task<IReadOnlyList<RedemptionRecord>> ListByUserAsync(Guid userId, CancellationToken ct = default)
		{
			return _repo.ListByUserAsync(userId, ct);
		}
	}
}
