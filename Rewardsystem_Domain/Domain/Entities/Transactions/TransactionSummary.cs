using System;
using Rewardsystem_Domain.Domain.Enums;

namespace Rewardsystem_Domain.Domain.Entities.Transactions
{
    // Lightweight projection for listing or reporting transactions.
    public sealed class TransactionSummary
    {
        // Identifier of the transaction.
        public Guid TransactionId { get; set; }

        // Monetary amount for the transaction.
        public decimal Amount { get; set; }

        // Date/time of the transaction.
        public DateTime Date { get; set; }

        // Type of transaction (purchase/refund etc).
        public TransactionType Type { get; set; }

        // Current transaction status.
        public TransactionStatus Status { get; set; }
    }
}
