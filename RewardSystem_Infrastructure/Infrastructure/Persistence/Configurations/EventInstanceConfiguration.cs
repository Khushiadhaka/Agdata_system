// Fluent configuration for EventInstance entity.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Configurations
{
    // Configures EventInstance table (scheduled events).
    public sealed class EventInstanceConfiguration : IEntityTypeConfiguration<EventInstance>
    {
        public void Configure(EntityTypeBuilder<EventInstance> builder)
        {
            // Table name.
            builder.ToTable("EventInstances");

            // Primary key.
            builder.HasKey(e => e.Id);

            // FK to EventDefinition.
            builder.Property(e => e.EventDefinitionId)
                   .IsRequired();

            // StartTime required.
            builder.Property(e => e.StartTime)
                   .IsRequired();

            // EndTime required.
            builder.Property(e => e.EndTime)
                   .IsRequired();

            // IsCompleted required.
            builder.Property(e => e.IsCompleted)
                   .IsRequired();

            // IsCancelled required.
            builder.Property(e => e.IsCancelled)
                   .IsRequired();

            // CreatedAt required.
            builder.Property(e => e.CreatedAt)
                   .IsRequired();

            // Many instances belong to one definition.
            builder.HasOne<EventDefinition>()
                   .WithMany()
                   .HasForeignKey(e => e.EventDefinitionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

