using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services.Interfaces;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Services.Implementations
{
    // Coordinates redemption requests with user account and product
    public sealed class RedemptionService : IRedemptionService
    {
        private readonly IRedemptionRequestRepository _requestRepository;
        private readonly IRedemptionRecordRepository _recordRepository;
        private readonly IRedemptionProcessRepository _processRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductInventoryRepository _inventoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RedemptionService(
            IRedemptionRequestRepository requestRepository,
            IRedemptionRecordRepository recordRepository,
            IRedemptionProcessRepository processRepository,
            IUserAccountRepository userAccountRepository,
            IProductRepository productRepository,
            IProductInventoryRepository inventoryRepository,
            IUnitOfWork unitOfWork)
        {
            _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
            _recordRepository = recordRepository ?? throw new ArgumentNullException(nameof(recordRepository));
            _processRepository = processRepository ?? throw new ArgumentNullException(nameof(processRepository));
            _userAccountRepository = userAccountRepository ?? throw new ArgumentNullException(nameof(userAccountRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        // Creates a new redemption request
        public async Task<RedemptionRequest> CreateRedemptionRequestAsync(
            Guid userId,
            Guid productId,
            int pointsUsed,
            CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty)
                throw new ValidationException("UserId cannot be empty.");

            if (productId == Guid.Empty)
                throw new ValidationException("ProductId cannot be empty.");

            var account = await _userAccountRepository.GetByUserIdAsync(userId, cancellationToken);
            if (account == null)
                throw new BusinessRuleException("User account not found.");

            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            if (product == null)
                throw new BusinessRuleException("Product not found.");

            var inventory = await _inventoryRepository.GetByProductIdAsync(productId, cancellationToken);
            if (inventory == null || inventory.StockQuantity <= 0)
                throw new BusinessRuleException("Product is out of stock.");

            if (pointsUsed < product.RequiredPoints)
                throw new BusinessRuleException("Not enough points provided for this product.");

            if (account.Points < pointsUsed)
                throw new BusinessRuleException("User does not have enough points.");

            var request = new RedemptionRequest(userId, productId, pointsUsed);

            await _requestRepository.AddAsync(request, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return request;
        }

        // Approve redemption
        public async Task ApproveRedemptionAsync(Guid redemptionRequestId, CancellationToken cancellationToken = default)
        {
            var request = await LoadRedemptionRequest(redemptionRequestId, cancellationToken);

            if (request.Status != RedemptionStatus.Pending)
                throw new BusinessRuleException("Only pending requests can be approved.");

            var account = await _userAccountRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (account == null)
                throw new BusinessRuleException("User account not found.");

            if (account.Points < request.PointsUsed)
                throw new BusinessRuleException("Insufficient points at approval time.");

            account.DeductPoints(request.PointsUsed);
            _userAccountRepository.Update(account);

            request.Approve();
            _requestRepository.Update(request);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Reject redemption
        public async Task RejectRedemptionAsync(Guid redemptionRequestId, CancellationToken cancellationToken = default)
        {
            var request = await LoadRedemptionRequest(redemptionRequestId, cancellationToken);

            if (request.Status != RedemptionStatus.Pending)
                throw new BusinessRuleException("Only pending requests can be rejected.");

            request.Reject();
            _requestRepository.Update(request);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Complete redemption (delivery done)
        public async Task CompleteRedemptionAsync(Guid redemptionRequestId, CancellationToken cancellationToken = default)
        {
            var request = await LoadRedemptionRequest(redemptionRequestId, cancellationToken);

            if (request.Status != RedemptionStatus.Approved)
                throw new BusinessRuleException("Only approved requests can be completed.");

            var inventory = await _inventoryRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
            if (inventory == null)
                throw new BusinessRuleException("Inventory record not found.");

            inventory.ReduceStock(1);
            _inventoryRepository.Update(inventory);

            var record = new RedemptionRecord(request.UserId, request.ProductId);
            await _recordRepository.AddAsync(record, cancellationToken);

            request.MarkCompleted();
            _requestRepository.Update(request);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // List user requests by status
        public async Task<IReadOnlyList<RedemptionRequest>> GetUserRequestsByStatusAsync(
            Guid userId,
            RedemptionStatus status,
            CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty)
                throw new ValidationException("UserId cannot be empty.");

            var allForUser = await _requestRepository.GetByUserIdAsync(userId, cancellationToken);
            return allForUser
                .Where(r => r.Status == status)
                .ToList();
        }

        // Helper to load request
        private async Task<RedemptionRequest> LoadRedemptionRequest(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new ValidationException("RedemptionRequestId cannot be empty.");

            var request = await _requestRepository.GetByIdAsync(id, cancellationToken);
            if (request == null)
                throw new BusinessRuleException("Redemption request not found.");

            return request;
        }
    }
}
