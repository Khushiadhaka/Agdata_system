namespace Rewardsystem_Domain.Domain.Common
{
    // Thrown when a business rule is violated
    public sealed class BusinessRuleException : DomainException
    {
        // Constructor with message
        public BusinessRuleException(string message) : base(message) { }
    }
}

