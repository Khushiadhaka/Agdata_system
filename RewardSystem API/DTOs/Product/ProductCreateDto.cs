namespace RewardSystem_API.DTOs.Product
{
    // Represents payload required to create a new product.
    public sealed class ProductCreateDto
    {
        // Product name.
        public string Name { get; set; } = string.Empty;

        // Optional description.
        public string? Description { get; set; }

        // Required points to redeem.
        public int RequiredPoints { get; set; }

        // Initial stock quantity for inventory.
        public int InitialStock { get; set; }

        // Optional SKU code.
        public string? SKU { get; set; }
    }
}
