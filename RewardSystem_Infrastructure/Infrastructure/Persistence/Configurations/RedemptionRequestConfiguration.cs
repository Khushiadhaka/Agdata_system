// Fluent configuration for RedemptionRequest entity.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rewardsystem_Domain.Domain.Entities.Redemption;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Configurations
{
    // Configures RedemptionRequest table (user requests).
    public sealed class RedemptionRequestConfiguration : IEntityTypeConfiguration<RedemptionRequest>
    {
        public void Configure(EntityTypeBuilder<RedemptionRequest> builder)
        {
            // Table name.
            builder.ToTable("RedemptionRequests");

            // Primary key.
            builder.HasKey(r => r.Id);

            // UserId required.
            builder.Property(r => r.UserId)
                   .IsRequired();

            // ProductId required.
            builder.Property(r => r.ProductId)
                   .IsRequired();

            // PointsUsed required.
            builder.Property(r => r.PointsUsed)
                   .IsRequired();

            // Status required.
            builder.Property(r => r.Status)
                   .IsRequired();

            // CreatedAt required.
            builder.Property(r => r.CreatedAt)
                   .IsRequired();
        }
    }
}

