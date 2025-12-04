// Fluent configuration for EventDefinition entity.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Configurations
{
    // Configures EventDefinition table (event templates).
    public sealed class EventDefinitionConfiguration : IEntityTypeConfiguration<EventDefinition>
    {
        public void Configure(EntityTypeBuilder<EventDefinition> builder)
        {
            // Table name.
            builder.ToTable("EventDefinitions");

            // Primary key.
            builder.HasKey(e => e.Id);

            // Name required.
            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            // Description optional.
            builder.Property(e => e.Description)
                   .HasMaxLength(1000);

            // RewardPoints required.
            builder.Property(e => e.RewardPoints)
                   .IsRequired();

            // IsActive required.
            builder.Property(e => e.IsActive)
                   .IsRequired();

            // CreatedAt required.
            builder.Property(e => e.CreatedAt)
                   .IsRequired();
        }
    }
}

