namespace Rewardsystem_Domain.Domain.Common
{
    // Thrown when input validation fails
    public sealed class ValidationException : DomainException
    {
        // Constructor with message
        public ValidationException(string message) : base(message) { }
    }
}

