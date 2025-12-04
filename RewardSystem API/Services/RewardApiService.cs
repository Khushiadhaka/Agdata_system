using AutoMapper;
using RewardSystem_API.DTOs.Reward;
using RewardSystem_Application.Interfaces.Reward;
using Rewardsystem_Domain.Domain.Entities.Reward;

namespace RewardSystem_API.Services
{
    public interface IRewardApiService
    {
        // Rewards
        Task<IReadOnlyList<RewardDto>> ListAsync(
            CancellationToken cancellationToken = default);

        Task<RewardDto?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<RewardDto> CreateAsync(
            RewardCreateDto dto,
            CancellationToken cancellationToken = default);

        Task<RewardDto?> UpdateAsync(
            Guid id,
            RewardUpdateDto dto,
            CancellationToken cancellationToken = default);

        // Points for a reward
        Task<RewardPointsDto?> GetRewardPointsAsync(
            Guid rewardId,
            CancellationToken cancellationToken = default);

        Task<RewardPointsDto> SetRewardPointsAsync(
            RewardPointsCreateDto dto,
            CancellationToken cancellationToken = default);

        // Top employees
        Task<IReadOnlyList<Top3EmployeeRewardDto>> GetTop3EmployeesAsync(
            CancellationToken cancellationToken = default);

        // Reward transactions
        Task<RewardTransactionDto> CreateRewardTransactionAsync(
            RewardTransactionCreateDto dto,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<RewardTransactionDto>> ListRewardTransactionsByUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }

    public sealed class RewardApiService : IRewardApiService
    {
        public Task<IReadOnlyList<RewardDto>> ListAsync(
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<RewardDto?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<RewardDto> CreateAsync(
            RewardCreateDto dto,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<RewardDto?> UpdateAsync(
            Guid id,
            RewardUpdateDto dto,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<RewardPointsDto?> GetRewardPointsAsync(
            Guid rewardId,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<RewardPointsDto> SetRewardPointsAsync(
            RewardPointsCreateDto dto,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<IReadOnlyList<Top3EmployeeRewardDto>> GetTop3EmployeesAsync(
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<RewardTransactionDto> CreateRewardTransactionAsync(
            RewardTransactionCreateDto dto,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<IReadOnlyList<RewardTransactionDto>> ListRewardTransactionsByUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();
    }
}

