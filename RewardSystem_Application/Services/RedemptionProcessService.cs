using RewardSystem_Application.Common;
using RewardSystem_Application.Interfaces.Redemption;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Services
{
    // Handles fulfillment: creates redemption records, deducts points and reduces stock when completing approved requests.
    public class RedemptionProcessService : IRedemptionProcessService
    {
        private readonly IRedemptionRequestRepository _requestRepo;
        private readonly IRedemptionRecordRepository _recordRepo;
        private readonly IUserAccountRepository _accountRepo;
        private readonly IProductInventoryRepository _inventoryRepo;
        private readonly IUnitOfWork _uow;

        public RedemptionProcessService(
            IRedemptionRequestRepository requestRepo,
            IRedemptionRecordRepository recordRepo,
            IUserAccountRepository accountRepo,
            IProductInventoryRepository inventoryRepo,
            IUnitOfWork uow)
        {
            _requestRepo = requestRepo ?? throw new ArgumentNullException(nameof(requestRepo));
            _recordRepo = recordRepo ?? throw new ArgumentNullException(nameof(recordRepo));
            _accountRepo = accountRepo ?? throw new ArgumentNullException(nameof(accountRepo));
            _inventoryRepo = inventoryRepo ?? throw new ArgumentNullException(nameof(inventoryRepo));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        // Create a fulfillment record (reduces stock and persists record).
        public async Task<Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRecord> CreateRecordAsync(Guid userId, Guid productId, string? reference = null, CancellationToken ct = default)
        {
            if (userId == Guid.Empty) throw new ValidationException("UserId required.");
            if (productId == Guid.Empty) throw new ValidationException("ProductId required.");

            var inv = await _inventoryRepo.GetByProductIdAsync(productId, ct) ?? throw new InvalidOperationException("Inventory not found.");
            if (inv.StockQuantity <= 0) throw new BusinessRuleException("Out of stock.");

            inv.ReduceStock(1);
            await _inventoryRepo.UpdateAsync(inv, ct);

            var rec = new Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRecord(userId, productId);
            await _recordRepo.AddAsync(rec, ct);

            await _uow.SaveChangesAsync(ct);
            return rec;
        }

        // Complete an approved redemption request: deduct points, reduce stock, mark completed, and record redemption.
        public async Task<Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRecord> CompleteRequestAsync(Guid requestId, string? reference = null, CancellationToken ct = default)
        {
            var req = await _requestRepo.GetByIdAsync(requestId, ct) ?? throw new InvalidOperationException("Request not found.");
            if (req.Status != Rewardsystem_Domain.Domain.Enums.RedemptionStatus.Approved)
                throw new BusinessRuleException("Only approved requests can be completed.");

            var productId = req.ProductId;
            var userId = req.UserId;

            var inv = await _inventoryRepo.GetByProductIdAsync(productId, ct) ?? throw new InvalidOperationException("Inventory not found.");
            if (inv.StockQuantity <= 0) throw new BusinessRuleException("Out of stock.");

            var acc = await _accountRepo.GetByUserIdAsync(userId, ct) ?? throw new InvalidOperationException("Account not found.");
            if (acc.Points < req.PointsUsed) throw new InsufficientPointsException("Insufficient points at completion.");

            acc.DeductPoints(req.PointsUsed);
            await _accountRepo.UpdateAsync(acc, ct);

            inv.ReduceStock(1);
            await _inventoryRepo.UpdateAsync(inv, ct);

            req.MarkCompleted();
            await _requestRepo.UpdateAsync(req, ct);

            var rec = new Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRecord(userId, productId);
            await _recordRepo.AddAsync(rec, ct);

            await _uow.SaveChangesAsync(ct);
            return rec;
        }
    }
}
