using RewardSystem_Application.Common;
using RewardSystem_Application.Interfaces.Redemption;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_Application.Services
{
	public class RedemptionRequestService : IRedemptionRequestService
	{
		private readonly IRedemptionRequestRepository _repo;
		private readonly IUnitOfWork _uow;

		public RedemptionRequestService(
			IRedemptionRequestRepository repo,
			IUnitOfWork uow)
		{
			_repo = repo;
			_uow = uow;
		}

		public async Task<RedemptionRequest> CreateAsync(
			Guid userId,
			Guid productId,
			int pointsUsed,
			CancellationToken ct = default)
		{
			if (userId == Guid.Empty)
				throw new ValidationException("UserId required.");

			if (productId == Guid.Empty)
				throw new ValidationException("ProductId required.");

			if (pointsUsed <= 0)
				throw new ValidationException("Points must be positive.");

			// ❗ Duplicate pending request prevention
			var exists = await _repo.ExistsPendingAsync(userId, productId, ct);
			if (exists)
				throw new BusinessRuleException("Pending redemption already exists for this product.");

			var request = new RedemptionRequest(userId, productId, pointsUsed);
			await _repo.AddAsync(request, ct);
			await _uow.SaveChangesAsync(ct);

			return request;
		}

		public async Task<RedemptionRequest> UpdateStatusAsync(
			Guid requestId,
			RedemptionStatus newStatus,
			string? note = null,
			CancellationToken ct = default)
		{
			var request = await _repo.GetByIdAsync(requestId, ct)
						  ?? throw new InvalidOperationException("Redemption request not found.");

			switch (newStatus)
			{
				case RedemptionStatus.Approved:
					request.Approve();
					break;

				case RedemptionStatus.Rejected:
					request.Reject(note);
					break;

				case RedemptionStatus.Cancelled:
					request.Cancel(note);
					break;

				case RedemptionStatus.Completed:
					request.MarkCompleted();
					break;

				default:
					throw new BusinessRuleException("Invalid status transition.");
			}

			await _repo.UpdateAsync(request, ct);
			await _uow.SaveChangesAsync(ct);
			return request;
		}

		public async Task<RedemptionRequest?> GetByIdAsync(Guid requestId, CancellationToken ct = default)
		{
			if (requestId == Guid.Empty) return null;
			return await _repo.GetByIdAsync(requestId, ct);
		}

		public async Task<IReadOnlyList<RedemptionRequest>> ListByUserAsync(Guid userId, CancellationToken ct = default)
		{
			return await _repo.ListByUserAsync(userId, ct);
		}
	}
}
