// Fluent configuration for PointsTransaction entity.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rewardsystem_Domain.Domain.Entities.Reward;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Configurations
{
    // Configures PointsTransaction table (earn/redeem history).
    public sealed class PointsTransactionConfiguration : IEntityTypeConfiguration<PointsTransaction>
    {
        public void Configure(EntityTypeBuilder<PointsTransaction> builder)
        {
            // Table name.
            builder.ToTable("PointsTransactions");

            // Primary key.
            builder.HasKey(pt => pt.Id);

            // UserId required.
            builder.Property(pt => pt.UserId)
                   .IsRequired();

            // Points required.
            builder.Property(pt => pt.Points)
                   .IsRequired();

            // Type required (Earn/Redeem/Adjust).
            builder.Property(pt => pt.Type)
                   .IsRequired();

            // Description optional.
            builder.Property(pt => pt.Description)
                   .HasMaxLength(500);

            // CreatedAt required.
            builder.Property(pt => pt.CreatedAt)
                   .IsRequired();
        }
    }
}

