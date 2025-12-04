using System;

namespace Rewardsystem_Domain.Domain.Common
{
    // Base class for all custom domain exceptions
    public abstract class DomainException : Exception
    {
        // Constructor accepts message
        protected DomainException(string message) : base(message) { }
    }
}

