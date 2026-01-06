// Fluent configuration for UserAccount entity.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Configurations
{
    // Configures UserAccount table and its properties.
    public sealed class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            // Table name.
            builder.ToTable("UserAccounts");

            // Primary key.
            builder.HasKey(a => a.Id);

            // Foreign key to User.
            builder.Property(a => a.UserId)
                   .IsRequired();

            // Points balance is required.
            builder.Property(a => a.Points)
                   .IsRequired();

            // Status is required (enum).
            builder.Property(a => a.Status)
                   .IsRequired();

            // CreatedAt required.
            builder.Property(a => a.CreatedAt)
                   .IsRequired();
        }
    }
}

