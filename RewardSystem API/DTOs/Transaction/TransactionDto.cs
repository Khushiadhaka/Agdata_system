namespace RewardSystem_API.DTOs.Transaction
{
    // Represents a business transaction returned to the client.
    public sealed class TransactionDto
    {
        // Unique identifier of the transaction.
        public Guid Id { get; set; }

        // User who performed the transaction.
        public Guid UserId { get; set; }

        // Optional product involved in the transaction.
        public Guid? ProductId { get; set; }

        // Monetary amount of the transaction.
        public decimal Amount { get; set; }

        // Reward points earned in this transaction.
        public int RewardPointsEarned { get; set; }

        // Type of transaction (domain enum as string).
        public string Type { get; set; } = string.Empty;

        // Current status of transaction (Pending/Completed/Failed).
        public string Status { get; set; } = string.Empty;

        // Date and time when the transaction occurred.
        public DateTime Date { get; set; }
    }
}
