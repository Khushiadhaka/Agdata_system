
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Configurations
{
    // Configures User table, value objects, and relations.
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table name.
            builder.ToTable("Users");

            // Primary key.
            builder.HasKey(u => u.Id);

            // Name is required.
            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            // Own Email as value object.
            builder.OwnsOne(u => u.Email, email =>
            {
                // Store as single column "Email".
                email.Property(e => e.Value)
                     .HasColumnName("Email")
                     .IsRequired()
                     .HasMaxLength(200);
            });

            // Own EmployeeId as value object.
            builder.OwnsOne(u => u.EmployeeId, emp =>
            {
                // Store as single column "EmployeeId".
                emp.Property(e => e.Value)
                   .HasColumnName("EmployeeId")
                   .IsRequired()
                   .HasMaxLength(50);
            });

            // Role is required (enum as int by default).
            builder.Property(u => u.Role)
                   .IsRequired();

            // Soft delete flag is required.
            builder.Property(u => u.IsDeleted)
                   .IsRequired();

            // CreatedAt required.
            builder.Property(u => u.CreatedAt)
                   .IsRequired();

            // One-to-one with UserAccount.
            builder.HasOne(u => u.Account)
                   .WithOne(a => a.User!)
                   .HasForeignKey<UserAccount>(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // One-to-one with UserProfile.
            builder.HasOne(u => u.Profile)
                   .WithOne(p => p.User!)
                   .HasForeignKey<UserProfile>(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

