using AutoMapper;
using RewardSystem_API.DTOs.Reward;
using RewardSystem_API.DTOs.Transaction;
using RewardSystem_Application.Interfaces.Transaction;
using Rewardsystem_Domain.Domain.Entities.Transactions;

namespace RewardSystem_API.Services
{
    /// <summary>
    /// Orchestrates transaction-related operations for the API layer.
    /// Wraps <see cref="ITransactionService"/> and maps DTOs & entities.
    /// </summary>
    public sealed class TransactionApiService : ITransactionApiService
    {
        private readonly ITransactionService _transactionService; // application service
        private readonly IMapper _mapper;                          // AutoMapper mapper

        public TransactionApiService(
            ITransactionService transactionService,
            IMapper mapper)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Creates a new business transaction and returns the created transaction DTO.
        /// </summary>
        public async Task<TransactionDto> CreateAsync(
            TransactionCreateDto dto,
            CancellationToken cancellationToken = default)
        {
            // Call into application layer with ALL required parameters.
            Transaction tx = await _transactionService.CreateAsync(
                dto.UserId,              // Guid userId
                dto.RelatedEntityId,     // Guid? relatedId
                dto.Amount,              // decimal amount  
                dto.RewardPoints,        // int rewardPoints
                dto.Type,                // TransactionType type
                cancellationToken);      // CancellationToken

            // Map domain entity → DTO for API response.
            return _mapper.Map<TransactionDto>(tx);
        }
    }
}