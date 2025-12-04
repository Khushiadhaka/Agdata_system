// Fluent configuration for Reward entity.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rewardsystem_Domain.Domain.Entities.Reward;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Configurations
{
    // Configures Reward table (reward master).
    public sealed class RewardConfiguration : IEntityTypeConfiguration<Reward>
    {
        public void Configure(EntityTypeBuilder<Reward> builder)
        {
            // Table name.
            builder.ToTable("Rewards");

            // Primary key.
            builder.HasKey(r => r.Id);

            // Name required.
            builder.Property(r => r.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            // Description optional.
            builder.Property(r => r.Description)
                   .HasMaxLength(1000);

            // Type required (enum).
            builder.Property(r => r.Type)
                   .IsRequired();

            // IsActive required.
            builder.Property(r => r.IsActive)
                   .IsRequired();

            // CreatedAt required.
            builder.Property(r => r.CreatedAt)
                   .IsRequired();
        }
    }
}

