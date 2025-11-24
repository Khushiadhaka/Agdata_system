using Rewardsystem_Domain.Domain.Enums;

namespace API.Api.Server.DTOs.Transactions
{
    public class CreateTransactionDto
    {
        public Guid UserId { get; set; }
        public Guid? ProductId { get; set; }
        public decimal Amount { get; set; }
        public int RewardPointsEarned { get; set; }
        public TransactionType Type { get; set; }
    }

    public class TransactionResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? ProductId { get; set; }
        public decimal Amount { get; set; }
        public int RewardPointsEarned { get; set; }
        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime Date { get; set; }
    }
}
