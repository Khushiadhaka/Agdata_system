using Rewardsystem_Domain.Domain.Common;

namespace Rewardsystem_Domain.Domain.Exceptions
{
    // Thrown when product is out of stock
    public sealed class ProductOutOfStock : DomainException
    {
        public ProductOutOfStock(string message) : base(message) { }
    }
}

