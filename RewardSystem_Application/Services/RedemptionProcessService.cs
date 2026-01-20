using System;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Common;
using RewardSystem_Application.Interfaces.Redemption;
using RewardSystem_Application.Interfaces.Users;
using RewardSystem_Application.Interfaces.Product;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_Application.Services
{
	/// <summary>
	/// Handles completion and fulfillment of approved redemption requests.
	/// </summary>
	public sealed class RedemptionProcessService : IRedemptionProcessService
	{
		private readonly IRedemptionRequestRepository _requestRepo;
		private readonly IRedemptionProcessRepository _processRepo;
		private readonly IRedemptionRecordRepository _recordRepo;
		private readonly IUserAccountService _accountService;
		private readonly IProductService _productService;
		private readonly IUnitOfWork _uow;

		public RedemptionProcessService(
			IRedemptionRequestRepository requestRepo,
			IRedemptionProcessRepository processRepo,
			IRedemptionRecordRepository recordRepo,
			IUserAccountService accountService,
			IProductService productService,
			IUnitOfWork uow)
		{
			_requestRepo = requestRepo ?? throw new ArgumentNullException(nameof(requestRepo));
			_processRepo = processRepo ?? throw new ArgumentNullException(nameof(processRepo));
			_recordRepo = recordRepo ?? throw new ArgumentNullException(nameof(recordRepo));
			_accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
			_productService = productService ?? throw new ArgumentNullException(nameof(productService));
			_uow = uow ?? throw new ArgumentNullException(nameof(uow));
		}

		/// <summary>
		/// Completes an approved redemption request.
		/// Deducts points, reduces stock, creates process + record.
		/// </summary>
		public async Task<RedemptionRecord> CompleteRequestAsync(
			Guid requestId,
			string? reference = null,
			CancellationToken ct = default)
		{
			if (requestId == Guid.Empty)
				throw new ValidationException("RequestId cannot be empty.");

			var request = await _requestRepo.GetByIdAsync(requestId, ct)
						  ?? throw new InvalidOperationException("Redemption request not found.");

			// Prevent double completion
			if (request.Status == RedemptionStatus.Completed)
				throw new BusinessRuleException("Redemption already completed.");

			if (request.Status != RedemptionStatus.Approved)
				throw new BusinessRuleException("Only approved requests can be completed.");

			// 1️⃣ Deduct points from user account
			var deducted = await _accountService.TryDeductPointsAsync(
				request.UserId,
				request.PointsUsed,
				"Product redemption",
				ct);

			if (!deducted)
				throw new BusinessRuleException("Insufficient points.");

			// 2️⃣ Reduce product stock (1 unit)
			await _productService.AdjustStockAsync(request.ProductId, -1, ct);

			// 3️⃣ Create redemption process (audit / lifecycle)
			var process = new RedemptionProcess(request.Id, request.PointsUsed);
			process.MarkCompleted();
			await _processRepo.AddAsync(process, ct);

			// 4️⃣ Create fulfillment record
			var record = new RedemptionRecord(
				request.UserId,
				request.ProductId,
				reference);

			await _recordRepo.AddAsync(record, ct);

			// 5️⃣ Mark request as completed
			request.MarkCompleted();
			await _requestRepo.UpdateAsync(request, ct);

			await _uow.SaveChangesAsync(ct);
			return record;
		}

		/// <summary>
		/// Creates a redemption record manually (admin / external fulfillment).
		/// </summary>
		public async Task<RedemptionRecord> CreateRecordAsync(
			Guid userId,
			Guid productId,
			string? reference = null,
			CancellationToken ct = default)
		{
			if (userId == Guid.Empty)
				throw new ValidationException("UserId cannot be empty.");

			if (productId == Guid.Empty)
				throw new ValidationException("ProductId cannot be empty.");

			var record = new RedemptionRecord(userId, productId, reference);
			await _recordRepo.AddAsync(record, ct);
			await _uow.SaveChangesAsync(ct);
			return record;
		}
	}
}
