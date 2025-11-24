using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Common
{
    // Thrown when validation rules are violated
    public sealed class ValidationException : DomainException
    {
        // Creates a new validation exception with message
        public ValidationException(string message) : base(message) { }
    }
}
