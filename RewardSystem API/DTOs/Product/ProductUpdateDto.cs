namespace RewardSystem_API.DTOs.Product
{
    // Represents payload used to update an existing product.
    public sealed class ProductUpdateDto
    {
        // Product id to update.
        public Guid Id { get; set; }

        // Updated name.
        public string Name { get; set; } = string.Empty;

        // Updated description.
        public string? Description { get; set; }

        // Updated points required.
        public int RequiredPoints { get; set; }

        // Updated SKU code.
        public string? SKU { get; set; }
    }
}
