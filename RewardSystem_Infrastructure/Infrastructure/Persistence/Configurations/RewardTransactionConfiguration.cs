// Fluent configuration for RewardTransaction entity.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rewardsystem_Domain.Domain.Entities.Reward;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Configurations
{
    // Configures RewardTransaction table (reward application to user).
    public sealed class RewardTransactionConfiguration : IEntityTypeConfiguration<RewardTransaction>
    {
        public void Configure(EntityTypeBuilder<RewardTransaction> builder)
        {
            // Table name.
            builder.ToTable("RewardTransactions");

            // Primary key.
            builder.HasKey(rt => rt.Id);

            // RewardId required.
            builder.Property(rt => rt.RewardId)
                   .IsRequired();

            // UserId required.
            builder.Property(rt => rt.UserId)
                   .IsRequired();

            // PointsGranted required.
            builder.Property(rt => rt.PointsGranted)
                   .IsRequired();

            // Reference optional.
            builder.Property(rt => rt.Reference)
                   .HasMaxLength(200);

            // TransactionType required.
            builder.Property(rt => rt.TransactionType)
                   .IsRequired();

            // Optional EventInstanceId and RedemptionRequestId.
            builder.Property(rt => rt.EventInstanceId);
            builder.Property(rt => rt.RedemptionRequestId);

            // CreatedAt required.
            builder.Property(rt => rt.CreatedAt)
                   .IsRequired();
        }
    }
}

