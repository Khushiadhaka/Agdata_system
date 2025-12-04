using Microsoft.EntityFrameworkCore;
using Rewardsystem_Domain.Domain.Entities.Event;
using Rewardsystem_Domain.Domain.Entities.Product;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Entities.Transactions;
using Rewardsystem_Domain.Domain.Entities.User;


namespace RewardSystem_Infrastructure.Infrastructure.Persistence
{
    // Main EF Core DbContext used for database access.
    public class RewardDbContext : DbContext
    {
        public RewardDbContext(DbContextOptions<RewardDbContext> options)
            : base(options)
        {
        }

        // Users table.
        public DbSet<User> Users => Set<User>();

        // User accounts table (points balances).
        public DbSet<UserAccount> UserAccounts => Set<UserAccount>();

        // User profiles table (phone, department, location).
        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

        // Products catalog table.
        public DbSet<Product> Products => Set<Product>();

        // Product inventory table (stock per product).
        public DbSet<ProductInventory> ProductInventories => Set<ProductInventory>();

        // Event definitions table (reusable event templates).
        public DbSet<EventDefinition> EventDefinitions => Set<EventDefinition>();

        // Event instances table (scheduled occurrences).
        public DbSet<EventInstance> EventInstances => Set<EventInstance>();

        // Event reward rules table (condition → points).
        public DbSet<EventRewardRule> EventRewardRules => Set<EventRewardRule>();

        // Reward table (reward definitions).
        public DbSet<Reward> Rewards => Set<Reward>();

        // Reward points table (points configuration for rewards).
        public DbSet<RewardPoints> RewardPoints => Set<RewardPoints>();

        // Reward transactions table (reward → user).
        public DbSet<RewardTransaction> RewardTransactions => Set<RewardTransaction>();

        // Points transactions table (earn / redeem history).
        public DbSet<PointsTransaction> PointsTransactions => Set<PointsTransaction>();

        // Redemption requests table (user requests to redeem).
        public DbSet<RedemptionRequest> RedemptionRequests => Set<RedemptionRequest>();

        // Redemption processes table (lifecycle of redemption).
        public DbSet<RedemptionProcess> RedemptionProcesses => Set<RedemptionProcess>();

        // Redemption records table (fulfilled redemptions).
        public DbSet<RedemptionRecord> RedemptionRecords => Set<RedemptionRecord>();

        // Business transactions table (generic business transaction).
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureUser(modelBuilder);
            ConfigureProduct(modelBuilder);
            ConfigureEvent(modelBuilder);
            ConfigureReward(modelBuilder);
            ConfigureRedemption(modelBuilder);
            ConfigureTransaction(modelBuilder);
        }

        // ---------------- USER ----------------
        private static void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.OwnsOne(x => x.Email, email =>
                {
                    email.Property(e => e.Value)
                        .HasColumnName("Email")
                        .IsRequired()
                        .HasMaxLength(200);
                });

                entity.OwnsOne(x => x.EmployeeId, emp =>
                {
                    emp.Property(e => e.Value)
                        .HasColumnName("EmployeeId")
                        .IsRequired()
                        .HasMaxLength(50);
                });

                entity.Property(x => x.CreatedAt).IsRequired();
                entity.Property(x => x.IsDeleted).IsRequired();

                entity.HasOne(x => x.Account)
                    .WithOne(a => a.User!)
                    .HasForeignKey<UserAccount>(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Profile)
                    .WithOne(p => p.User!)
                    .HasForeignKey<UserProfile>(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.UserId).IsRequired();
                entity.Property(x => x.Points).IsRequired();
                entity.Property(x => x.PasswordHash).HasMaxLength(500);
                entity.Property(x => x.Status).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.UserId).IsRequired();
                entity.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(50);
                entity.Property(x => x.Department).IsRequired().HasMaxLength(100);
                entity.Property(x => x.Location).IsRequired().HasMaxLength(100);
                entity.Property(x => x.CreatedAt).IsRequired();
            });
        }

        // ---------------- PRODUCT + INVENTORY ----------------
        private static void ConfigureProduct(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.Description)
                    .HasMaxLength(1000);

                entity.Property(x => x.RequiredPoints)
                    .IsRequired();

                entity.Property(x => x.IsActive)
                    .IsRequired();

                entity.Property(x => x.CreatedAt)
                    .IsRequired();

                // ✅ SKU value object ko owned type ki tarah map karo
                entity.OwnsOne(x => x.Sku, sku =>
                {
                    // yahan SKU ki string property ko column se map kar rahe hain
                    sku.Property(s => s.Value)           // agar property ka naam Code hai to .Code kar dena
                       .HasColumnName("Sku")
                       .HasMaxLength(50)
                       .IsRequired();
                });
            });

            modelBuilder.Entity<ProductInventory>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.ProductId).IsRequired();
                entity.Property(x => x.StockQuantity).IsRequired();
                entity.Property(x => x.IsActive).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();

                entity.HasOne<Product>()
                    .WithOne()
                    .HasForeignKey<ProductInventory>(pi => pi.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        // ---------------- EVENTS ----------------
        private static void ConfigureEvent(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventDefinition>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Description).HasMaxLength(1000);
                entity.Property(x => x.RewardPoints).IsRequired();
                entity.Property(x => x.IsActive).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<EventInstance>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.EventDefinitionId).IsRequired();
                entity.Property(x => x.StartTime).IsRequired();
                entity.Property(x => x.EndTime).IsRequired();
                entity.Property(x => x.IsCompleted).IsRequired();
                entity.Property(x => x.IsCancelled).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();

                entity.HasOne<EventDefinition>()
                    .WithMany()
                    .HasForeignKey(x => x.EventDefinitionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EventRewardRule>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.EventDefinitionId).IsRequired();
                entity.Property(x => x.Condition).IsRequired().HasMaxLength(500);
                entity.Property(x => x.Points).IsRequired();
                entity.Property(x => x.IsActive).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();

                entity.HasOne<EventDefinition>()
                    .WithMany()
                    .HasForeignKey(x => x.EventDefinitionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        // ---------------- REWARD ----------------
        private static void ConfigureReward(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reward>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Description).HasMaxLength(1000);
                entity.Property(x => x.Type).IsRequired();
                entity.Property(x => x.IsActive).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<RewardPoints>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.RewardId).IsRequired();
                entity.Property(x => x.Points).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();

                entity.HasOne<Reward>()
                    .WithMany()
                    .HasForeignKey(x => x.RewardId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RewardTransaction>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.RewardId).IsRequired();
                entity.Property(x => x.UserId).IsRequired();
                entity.Property(x => x.PointsGranted).IsRequired();
                entity.Property(x => x.TransactionType).IsRequired();
                entity.Property(x => x.Reference).HasMaxLength(200);
                entity.Property(x => x.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<PointsTransaction>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.UserId).IsRequired();
                entity.Property(x => x.Points).IsRequired();
                entity.Property(x => x.Type).IsRequired();
                entity.Property(x => x.Description).HasMaxLength(500);
                entity.Property(x => x.CreatedAt).IsRequired();
            });
        }

        // ---------------- REDEMPTION ----------------
        private static void ConfigureRedemption(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RedemptionRequest>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.UserId).IsRequired();
                entity.Property(x => x.ProductId).IsRequired();
                entity.Property(x => x.PointsUsed).IsRequired();
                entity.Property(x => x.Status).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<RedemptionProcess>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.RedemptionId).IsRequired();
                entity.Property(x => x.PointsUsed).IsRequired();
                entity.Property(x => x.Status).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<RedemptionRecord>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.UserId).IsRequired();
                entity.Property(x => x.ProductId).IsRequired();
                entity.Property(x => x.RedeemedAt).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();
            });
        }

        // ---------------- TRANSACTION ----------------
        private static void ConfigureTransaction(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.UserId).IsRequired();
                entity.Property(x => x.Amount).IsRequired();
                entity.Property(x => x.RewardPointsEarned).IsRequired();
                entity.Property(x => x.Type).IsRequired();
                entity.Property(x => x.Status).IsRequired();
                entity.Property(x => x.Date).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();
            });
        }
    }
}
