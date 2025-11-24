using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Common
{
    // Thrown when a business rule is violated
    public sealed class BusinessRuleException : DomainException
    {
        // Creates a new business rule exception with message
        public BusinessRuleException(string message) : base(message) { }
    }
}
