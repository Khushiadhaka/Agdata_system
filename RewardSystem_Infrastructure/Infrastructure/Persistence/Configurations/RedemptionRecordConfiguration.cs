// Fluent configuration for RedemptionRecord entity.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rewardsystem_Domain.Domain.Entities.Redemption;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Configurations
{
    // Configures RedemptionRecord table (fulfilled redemptions).
    public sealed class RedemptionRecordConfiguration : IEntityTypeConfiguration<RedemptionRecord>
    {
        public void Configure(EntityTypeBuilder<RedemptionRecord> builder)
        {
            // Table name.
            builder.ToTable("RedemptionRecords");

            // Primary key.
            builder.HasKey(r => r.Id);

            // UserId required.
            builder.Property(r => r.UserId)
                   .IsRequired();

            // ProductId required.
            builder.Property(r => r.ProductId)
                   .IsRequired();

            // RedeemedAt required.
            builder.Property(r => r.RedeemedAt)
                   .IsRequired();

            // CreatedAt required.
            builder.Property(r => r.CreatedAt)
                   .IsRequired();
        }
    }
}

