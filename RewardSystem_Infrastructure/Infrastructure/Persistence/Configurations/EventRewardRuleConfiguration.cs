// Fluent configuration for EventRewardRule entity.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Configurations
{
    // Configures EventRewardRule table (conditions → points).
    public sealed class EventRewardRuleConfiguration : IEntityTypeConfiguration<EventRewardRule>
    {
        public void Configure(EntityTypeBuilder<EventRewardRule> builder)
        {
            // Table name.
            builder.ToTable("EventRewardRules");

            // Primary key.
            builder.HasKey(r => r.Id);

            // FK to EventDefinition.
            builder.Property(r => r.EventDefinitionId)
                   .IsRequired();

            // Condition required.
            builder.Property(r => r.Condition)
                   .IsRequired()
                   .HasMaxLength(500);

            // Points required.
            builder.Property(r => r.Points)
                   .IsRequired();

            // IsActive required.
            builder.Property(r => r.IsActive)
                   .IsRequired();

            // CreatedAt required.
            builder.Property(r => r.CreatedAt)
                   .IsRequired();

            // Many rules per definition.
            builder.HasOne<EventDefinition>()
                   .WithMany()
                   .HasForeignKey(r => r.EventDefinitionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

