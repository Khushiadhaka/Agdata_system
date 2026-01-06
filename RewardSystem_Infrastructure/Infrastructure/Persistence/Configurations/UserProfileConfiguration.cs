// Fluent configuration for UserProfile entity.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Configurations
{
    // Configures UserProfile table and its properties.
    public sealed class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            // Table name.
            builder.ToTable("UserProfiles");

            // Primary key.
            builder.HasKey(p => p.Id);

            // Foreign key to User.
            builder.Property(p => p.UserId)
                   .IsRequired();

            // Phone number required.
            builder.Property(p => p.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            // Department required.
            builder.Property(p => p.Department)
                   .IsRequired()
                   .HasMaxLength(100);

            // Location required.
            builder.Property(p => p.Location)
                   .IsRequired()
                   .HasMaxLength(100);

            // CreatedAt required.
            builder.Property(p => p.CreatedAt)
                   .IsRequired();
        }
    }
}

