namespace RewardSystem_API.DTOs.Product
{
    // Represents product inventory information.
    public sealed class ProductInventoryDto
    {
        // Unique inventory id.
        public Guid Id { get; set; }

        // Associated product id.
        public Guid ProductId { get; set; }

        // Available stock quantity.
        public int StockQuantity { get; set; }

        // Inventory active state.
        public bool IsActive { get; set; }
    }
}
