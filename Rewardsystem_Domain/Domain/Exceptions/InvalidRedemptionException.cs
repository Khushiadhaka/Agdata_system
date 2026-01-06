using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Exceptions
{
    // Thrown when redemption process violates rules
    public sealed class InvalidRedemptionException : DomainException
    {
        public InvalidRedemptionException(string message) : base(message) { }
    }
}

