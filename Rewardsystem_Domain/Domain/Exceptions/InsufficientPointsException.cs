using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Exceptions
{
    // Thrown when user does not have enough points to redeem
    public sealed class InsufficientPointsException : DomainException
    {
        public InsufficientPointsException(string message) : base(message) { }
    }
}

