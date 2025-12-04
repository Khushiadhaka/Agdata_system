// Fluent configuration for RedemptionProcess entity.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rewardsystem_Domain.Domain.Entities.Redemption;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Configurations
{
    // Configures RedemptionProcess table (lifecycle of redemption).
    public sealed class RedemptionProcessConfiguration : IEntityTypeConfiguration<RedemptionProcess>
    {
        public void Configure(EntityTypeBuilder<RedemptionProcess> builder)
        {
            // Table name.
            builder.ToTable("RedemptionProcesses");

            // Primary key.
            builder.HasKey(r => r.Id);

            // Business RedemptionId required.
            builder.Property(r => r.RedemptionId)
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

