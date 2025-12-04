// Fluent configuration for Product entity.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rewardsystem_Domain.Domain.Entities.Product;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Configurations
{
    // Configures Product catalog table.
    public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Table name.
            builder.ToTable("Products");

            // Primary key.
            builder.HasKey(p => p.Id);

            // Name required.
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            // Description optional.
            builder.Property(p => p.Description)
                   .HasMaxLength(1000);

            // Required points required.
            builder.Property(p => p.RequiredPoints)
                   .IsRequired();

            // IsActive required.
            builder.Property(p => p.IsActive)
                   .IsRequired();

            // CreatedAt required.
            builder.Property(p => p.CreatedAt)
                   .IsRequired();
        }
    }
}

