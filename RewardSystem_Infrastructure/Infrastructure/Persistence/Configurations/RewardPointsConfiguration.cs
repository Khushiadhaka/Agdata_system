// Fluent configuration for RewardPoints entity.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rewardsystem_Domain.Domain.Entities.Reward;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Configurations
{
    // Configures RewardPoints table (reward points versions).
    public sealed class RewardPointsConfiguration : IEntityTypeConfiguration<RewardPoints>
    {
        public void Configure(EntityTypeBuilder<RewardPoints> builder)
        {
            // Table name.
            builder.ToTable("RewardPoints");

            // Primary key.
            builder.HasKey(rp => rp.Id);

            // FK to Reward.
            builder.Property(rp => rp.RewardId)
                   .IsRequired();

            // Points required.
            builder.Property(rp => rp.Points)
                   .IsRequired();

            // Effective dates optional.
            builder.Property(rp => rp.EffectiveFrom);
            builder.Property(rp => rp.EffectiveTo);

            // CreatedAt required.
            builder.Property(rp => rp.CreatedAt)
                   .IsRequired();

            // Many points configs per reward.
            builder.HasOne<Reward>()
                   .WithMany()
                   .HasForeignKey(rp => rp.RewardId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

