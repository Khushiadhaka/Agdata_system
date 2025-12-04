namespace RewardSystem_API.DTOs.Product
{
    // Represents product information returned to clients.
    public sealed class ProductDto
    {
        // Unique identifier of the product.
        public Guid Id { get; set; }

        // Product name.
        public string Name { get; set; } = string.Empty;

        // Product detailed description.
        public string? Description { get; set; }

        // Reward points required to redeem this product.
        public int RequiredPoints { get; set; }

        // Indicates if product is active.
        public bool IsActive { get; set; }
        public string? SKU { get; set; }
    }
}
