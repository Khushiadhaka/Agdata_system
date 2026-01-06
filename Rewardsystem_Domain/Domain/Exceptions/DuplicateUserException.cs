using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Exceptions
{
    // Thrown when duplicate email/employeeId is found
    public sealed class DuplicateUserException : DomainException
    {
        public DuplicateUserException(string message) : base(message) { }
    }
}

