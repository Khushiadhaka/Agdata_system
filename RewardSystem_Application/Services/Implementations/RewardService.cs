using RewardSystem_Application.Common;
using RewardSystem_Application.Repositories;
using RewardSystem_Application.Services.Interfaces;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Services.Implementations
{
    // Application service for reward configuration and reward points
    public sealed class RewardService : IRewardService
    {
        // Repository for Reward aggregate
        private readonly IRewardRepository _rewardRepository;

        // Repository for RewardPoints configuration
        private readonly IRewardPointsRepository _rewardPointsRepository;

        // Unit of work for committing changes
        private readonly IUnitOfWork _unitOfWork;

        // Constructor with dependency injection
        public RewardService(
            IRewardRepository rewardRepository,
            IRewardPointsRepository rewardPointsRepository,
            IUnitOfWork unitOfWork)
        {
            _rewardRepository = rewardRepository ?? throw new ArgumentNullException(nameof(rewardRepository));
            _rewardPointsRepository = rewardPointsRepository ?? throw new ArgumentNullException(nameof(rewardPointsRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        // Create a new reward program/rule
        public async Task<Reward> CreateRewardAsync(
            string name,
            string description,
            RewardType type,
            CancellationToken cancellationToken = default)
        {
            // Domain entity will validate input
            var reward = new Reward(name, description, type);

            // Persist reward
            await _rewardRepository.AddAsync(reward, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Return created entity
            return reward;
        }

        // Get a single reward by its id
        public Task<Reward?> GetRewardByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ValidationException("RewardId cannot be empty.");

            // Delegate to repository
            return _rewardRepository.GetByIdAsync(id, cancellationToken);
        }

        // Get all rewards
        public Task<IReadOnlyList<Reward>> GetAllRewardsAsync(
            CancellationToken cancellationToken = default)
        {
            // Simple pass-through to repository
            return _rewardRepository.GetAllAsync(cancellationToken);
        }

        // Update an existing reward
        public async Task UpdateRewardAsync(
            Guid id,
            string name,
            string description,
            RewardType type,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ValidationException("RewardId cannot be empty.");

            // Load reward from repository
            var reward = await _rewardRepository.GetByIdAsync(id, cancellationToken);
            if (reward == null)
                throw new BusinessRuleException("Reward not found.");

            // Use domain method to update
            reward.Update(name, description, type);

            // Mark as updated
            _rewardRepository.Update(reward);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Deactivate a reward so it can no longer be used
        public async Task DeactivateRewardAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ValidationException("RewardId cannot be empty.");

            // Load reward
            var reward = await _rewardRepository.GetByIdAsync(id, cancellationToken);
            if (reward == null)
                throw new BusinessRuleException("Reward not found.");

            // Domain rule: deactivate
            reward.Deactivate();

            // Persist change
            _rewardRepository.Update(reward);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Create or update reward points configuration for a reward
        public async Task<RewardPoints> ConfigureRewardPointsAsync(
            Guid rewardId,
            int points,
            DateTime? effectiveFrom,
            DateTime? effectiveTo,
            CancellationToken cancellationToken = default)
        {
            if (rewardId == Guid.Empty)
                throw new ValidationException("RewardId cannot be empty.");

            // Ensure reward exists
            var reward = await _rewardRepository.GetByIdAsync(rewardId, cancellationToken);
            if (reward == null)
                throw new BusinessRuleException("Reward not found.");

            // Try to get existing points configuration
            var existing = await _rewardPointsRepository.GetByRewardIdAsync(rewardId, cancellationToken);

            // No config yet → create new
            if (existing == null)
            {
                var rewardPoints = new RewardPoints(rewardId, points, effectiveFrom, effectiveTo);

                await _rewardPointsRepository.AddAsync(rewardPoints, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return rewardPoints;
            }

            // Config exists → update it
            existing.UpdatePoints(points, effectiveFrom, effectiveTo);

            _rewardPointsRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return existing;
        }
    }
}
