using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Common;
using RewardSystem_Application.Interfaces.Redemption;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Exceptions;

namespace RewardSystem_Application.Services
{
    // Handles creation and status updates for redemption requests (validates balance and product availability).
    public class RedemptionRequestService : IRedemptionRequestService
    {
        // Repositories and unit of work used by this service.
        private readonly IRedemptionRequestRepository _requestRepo;
        private readonly IProductRepository _productRepo;
        private readonly IProductInventoryRepository _inventoryRepo;
        private readonly IUserAccountRepository _accountRepo;
        private readonly IRedemptionProcessRepository _processRepo;
        private readonly IUnitOfWork _uow;

        // Constructor with dependencies injected.
        public RedemptionRequestService(
            IRedemptionRequestRepository requestRepo,
            IProductRepository productRepo,
            IProductInventoryRepository inventoryRepo,
            IUserAccountRepository accountRepo,
            IRedemptionProcessRepository processRepo,
            IUnitOfWork uow)
        {
            _requestRepo = requestRepo ?? throw new ArgumentNullException(nameof(requestRepo));
            _productRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));
            _inventoryRepo = inventoryRepo ?? throw new ArgumentNullException(nameof(inventoryRepo));
            _accountRepo = accountRepo ?? throw new ArgumentNullException(nameof(accountRepo));
            _processRepo = processRepo ?? throw new ArgumentNullException(nameof(processRepo));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        // Create a new redemption request (validates user balance and product availability).
        public async Task<Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRequest> CreateAsync(
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
                throw new ValidationException("PointsUsed must be positive.");

            // Check product exists and is active.
            var product = await _productRepo.GetByIdAsync(productId, ct)
                          ?? throw new InvalidOperationException("Product not found.");
            if (!product.IsActive)
                throw new BusinessRuleException("Product not available.");

            // Check inventory using ProductInventoryRepository (correct place).
            var inv = await _inventoryRepo.GetByProductIdAsync(productId, ct);
            if (inv != null && inv.StockQuantity <= 0)
                throw new BusinessRuleException("Product out of stock.");

            // Check user account and balance.
            var acc = await _accountRepo.GetByUserIdAsync(userId, ct)
                      ?? throw new InvalidOperationException("Account not found.");
            if (acc.Points < pointsUsed)
                throw new InsufficientPointsException("Insufficient points.");

            // Prevent duplicate pending request for same user + product.
            var existing = await _requestRepo.GetPendingByUserAndProductAsync(userId, productId, ct);
            if (existing != null)
                throw new BusinessRuleException("A pending redemption already exists for this product.");

            // Create and persist new request.
            var req = new Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRequest(
                userId,
                productId,
                pointsUsed);

            await _requestRepo.AddAsync(req, ct);
            await _uow.SaveChangesAsync(ct);

            return req;
        }

        // Update status of a redemption request (approve / reject / complete).
        public async Task<Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRequest> UpdateStatusAsync(
            Guid requestId,
            Rewardsystem_Domain.Domain.Enums.RedemptionStatus newStatus,
            string? note = null,
            CancellationToken ct = default)
        {
            var req = await _requestRepo.GetByIdAsync(requestId, ct)
                      ?? throw new InvalidOperationException("Request not found.");

            switch (newStatus)
            {
                case Rewardsystem_Domain.Domain.Enums.RedemptionStatus.Approved:
                    req.Approve();
                    break;

                case Rewardsystem_Domain.Domain.Enums.RedemptionStatus.Rejected:
                    req.Reject();
                    break;

                case Rewardsystem_Domain.Domain.Enums.RedemptionStatus.Completed:
                    req.MarkCompleted();
                    break;

                default:
                    throw new BusinessRuleException("Invalid status transition.");
            }

            await _requestRepo.UpdateAsync(req, ct);
            await _uow.SaveChangesAsync(ct);

            return req;
        }

        // Get a redemption request by id.
        public async Task<Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRequest?> GetByIdAsync(
            Guid requestId,
            CancellationToken ct = default)
        {
            if (requestId == Guid.Empty)
                return null;

            return await _requestRepo.GetByIdAsync(requestId, ct);
        }

        // List requests created by a user.
        public async Task<IReadOnlyList<Rewardsystem_Domain.Domain.Entities.Redemption.RedemptionRequest>> ListByUserAsync(
            Guid userId,
            CancellationToken ct = default)
        {
            var list = await _requestRepo.GetByUserIdAsync(userId, ct);
            return list.ToList();
        }
    }
}
