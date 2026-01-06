using RewardSystem_API.DTOs.Transaction;

namespace RewardSystem_API.Services
{
    /// <summary>
    /// API-level abstraction for transaction operations.
    /// Wraps the application-layer ITransactionService and works with DTOs.
    /// </summary>
    public interface ITransactionApiService
    {
        /// <summary>
        /// Creates a new transaction and returns the created transaction DTO.
        /// </summary>
        /// <param name="dto">Incoming transaction data from the client.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created transaction as a DTO.</returns>
        Task<TransactionDto> CreateAsync(
            TransactionCreateDto dto,
            CancellationToken cancellationToken = default);
    }
}
