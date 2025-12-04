using System;

namespace Rewardsystem_Domain.Domain.Entities.Transactions
{
    // Additional details associated with a transaction (auxiliary, not an aggregate root).
    public sealed class TransactionDetail
    {
        // Identifier of the main transaction this detail belongs to.
        public Guid TransactionId { get; set; }

        // Human-readable description for auditing or display.
        public string Description { get; set; } = string.Empty;

        // Payment method used for the transaction (card, cash, etc).
        public string PaymentMethod { get; set; } = string.Empty;

        // External reference number (payment gateway id, invoice id).
        public string ReferenceNumber { get; set; } = string.Empty;
    }
}
