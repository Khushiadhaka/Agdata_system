// Fluent configuration for ProductInventory entity.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rewardsystem_Domain.Domain.Entities.Product;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Configurations
{
    // Configures ProductInventory stock table.
    public sealed class ProductInventoryConfiguration : IEntityTypeConfiguration<ProductInventory>
    {
        public void Configure(EntityTypeBuilder<ProductInventory> builder)
        {
            // Table name.
            builder.ToTable("ProductInventories");

            // Primary key.
            builder.HasKey(pi => pi.Id);

            // FK to Product.
            builder.Property(pi => pi.ProductId)
                   .IsRequired();

            // Stock quantity required.
            builder.Property(pi => pi.StockQuantity)
                   .IsRequired();

            // IsActive required.
            builder.Property(pi => pi.IsActive)
                   .IsRequired();

            // CreatedAt required.
            builder.Property(pi => pi.CreatedAt)
                   .IsRequired();

            // One-to-one relationship with Product.
            builder.HasOne<Product>()
                   .WithOne()
                   .HasForeignKey<ProductInventory>(pi => pi.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

