using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Entities.Transactions
{
    // Additional details associated with a transaction
    public sealed class TransactionDetail
    {
        // Identifier of the transaction
        public Guid TransactionId { get; set; }

        // Human-readable description
        public string Description { get; set; } = string.Empty;

        // Payment method used
        public string PaymentMethod { get; set; } = string.Empty;

        // External reference number
        public string ReferenceNumber { get; set; } = string.Empty;
    }
}
