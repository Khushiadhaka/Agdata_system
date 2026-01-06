// Fluent configuration for Transaction entity.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rewardsystem_Domain.Domain.Entities.Transactions;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence.Configurations
{
    // Configures Transaction table (business transactions that may earn points).
    public sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            // Table name.
            builder.ToTable("Transactions");

            // Primary key.
            builder.HasKey(t => t.Id);

            // UserId required.
            builder.Property(t => t.UserId)
                   .IsRequired();

            // ProductId optional.
            builder.Property(t => t.ProductId);

            // Amount required.
            builder.Property(t => t.Amount)
                   .IsRequired();

            // RewardPointsEarned required (can be 0).
            builder.Property(t => t.RewardPointsEarned)
                   .IsRequired();

            // Type required.
            builder.Property(t => t.Type)
                   .IsRequired();

            // Status required.
            builder.Property(t => t.Status)
                   .IsRequired();

            // Date required.
            builder.Property(t => t.Date)
                   .IsRequired();

            // CreatedAt required.
            builder.Property(t => t.CreatedAt)
                   .IsRequired();
        }
    }
}

