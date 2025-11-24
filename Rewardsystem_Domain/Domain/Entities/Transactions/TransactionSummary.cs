using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Entities.Transactions
{
    // Lightweight summary for transactions
    public sealed class TransactionSummary
    {
        // Identifier of the transaction
        public Guid TransactionId { get; set; }

        // Monetary amount
        public decimal Amount { get; set; }

        // Date and time of the transaction
        public DateTime Date { get; set; }

        // Type of the transaction
        public TransactionType Type { get; set; }

        // Current status of the transaction
        public TransactionStatus Status { get; set; }
    }
}
