using System;
using System.Collections.Generic;
using System.Text;

namespace Rewardsystem_Domain.Domain.Common
{
    // Base exception type for domain errors
    public abstract class DomainException : Exception
    {
        // Creates a new domain exception with message
        protected DomainException(string message) : base(message) { }
    }
}
