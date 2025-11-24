using Microsoft.EntityFrameworkCore;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Event;
using Rewardsystem_Domain.Domain.Entities.Product;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Entities.Transactions;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Enums;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence
{
    public class RewardDbContext : DbContext
    {
        public RewardDbContext(DbContextOptions<RewardDbContext> options) : base(options) { }

        // Users
        public DbSet<User> Users => Set<User>();
        public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

        // Events
        public DbSet<Event> Events => Set<Event>();
        public DbSet<EventDefinition> EventDefinitions => Set<EventDefinition>();
        public DbSet<EventInstance> EventInstances => Set<EventInstance>();
        public DbSet<EventRewardRule> EventRewardRules => Set<EventRewardRule>();

        // Products
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductInventory> ProductInventories => Set<ProductInventory>();

        // Rewards & points
        public DbSet<Reward> Rewards => Set<Reward>();
        public DbSet<RewardPoints> RewardPoints => Set<RewardPoints>();
        public DbSet<RewardTransaction> RewardTransactions => Set<RewardTransaction>();
        public DbSet<PointsTransaction> PointsTransactions => Set<PointsTransaction>();

        // Redemptions
        public DbSet<RedemptionRequest> RedemptionRequests => Set<RedemptionRequest>();
        public DbSet<RedemptionRecord> RedemptionRecords => Set<RedemptionRecord>();
        public DbSet<RedemptionProcess> RedemptionProcesses => Set<RedemptionProcess>();

        // Transactions
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // BaseEntity common config 
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // CreatedAt default value
                var createdAtProp = entityType.FindProperty(nameof(BaseEntity.CreatedAt));
                if (createdAtProp != null)
                {
                    createdAtProp.SetDefaultValueSql("GETUTCDATE()");
                }
            }

            // USERS
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(u => u.EmployeeId)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(u => u.Role)
                      .HasConversion<int>()
                      .IsRequired();
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.ToTable("UserAccounts");
                entity.HasKey(a => a.Id);

                entity.Property(a => a.UserId)
                      .IsRequired();

                entity.Property(a => a.Points)
                      .IsRequired();

                entity.Property(a => a.Status)
                      .HasConversion<int>()
                      .IsRequired();

                //  1-to-1 between User and UserAccount if you want
                entity.HasOne(a => a.User)
                      .WithOne(u => u.Account)
                      .HasForeignKey<UserAccount>(a => a.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.ToTable("UserProfiles");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.UserId).IsRequired();
                entity.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(50);
                entity.Property(p => p.Department).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Location).IsRequired().HasMaxLength(100);
            });

            //EVENTS
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Events");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            modelBuilder.Entity<EventDefinition>(entity =>
            {
                entity.ToTable("EventDefinitions");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.RewardPoints).IsRequired();
            });

            modelBuilder.Entity<EventInstance>(entity =>
            {
                entity.ToTable("EventInstances");
                entity.HasKey(ei => ei.Id);

                entity.Property(ei => ei.EventDefinitionId).IsRequired();
                entity.Property(ei => ei.StartTime).IsRequired();
                entity.Property(ei => ei.EndTime).IsRequired();
            });

            modelBuilder.Entity<EventRewardRule>(entity =>
            {
                entity.ToTable("EventRewardRules");
                entity.HasKey(r => r.Id);

                entity.Property(r => r.EventDefinitionId).IsRequired();
                entity.Property(r => r.Condition).IsRequired().HasMaxLength(300);
                entity.Property(r => r.Points).IsRequired();
            });

            //PRODUCTS
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Description).HasMaxLength(500);
                entity.Property(p => p.RequiredPoints).IsRequired();
            });

            modelBuilder.Entity<ProductInventory>(entity =>
            {
                entity.ToTable("ProductInventories");
                entity.HasKey(pi => pi.Id);

                entity.Property(pi => pi.ProductId).IsRequired();
                entity.Property(pi => pi.StockQuantity).IsRequired();
                entity.Property(pi => pi.IsActive).IsRequired();


            });

            //REWARDS & POINTS
            modelBuilder.Entity<Reward>(entity =>
            {
                entity.ToTable("Rewards");
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Name).IsRequired().HasMaxLength(200);
                entity.Property(r => r.Description).HasMaxLength(500);
                entity.Property(r => r.Type)
                      .HasConversion<int>()
                      .IsRequired();
            });

            modelBuilder.Entity<RewardPoints>(entity =>
            {
                entity.ToTable("RewardPoints");
                entity.HasKey(rp => rp.Id);

                entity.Property(rp => rp.RewardId).IsRequired();
                entity.Property(rp => rp.Points).IsRequired();
            });

            modelBuilder.Entity<RewardTransaction>(entity =>
            {
                entity.ToTable("RewardTransactions");
                entity.HasKey(rt => rt.Id);

                entity.Property(rt => rt.RewardId).IsRequired();
                entity.Property(rt => rt.UserId).IsRequired();
                entity.Property(rt => rt.PointsGranted).IsRequired();
                entity.Property(rt => rt.TransactionType)
                      .HasConversion<int>()
                      .IsRequired();
            });

            modelBuilder.Entity<PointsTransaction>(entity =>
            {
                entity.ToTable("PointsTransactions");
                entity.HasKey(pt => pt.Id);

                entity.Property(pt => pt.UserId).IsRequired();
                entity.Property(pt => pt.Points).IsRequired();
                entity.Property(pt => pt.Type)
                      .HasConversion<int>()
                      .IsRequired();
            });

            //REDEMPTIONS
            modelBuilder.Entity<RedemptionRequest>(entity =>
            {
                entity.ToTable("RedemptionRequests");
                entity.HasKey(r => r.Id);

                entity.Property(r => r.UserId).IsRequired();
                entity.Property(r => r.ProductId).IsRequired();
                entity.Property(r => r.PointsUsed).IsRequired();
                entity.Property(r => r.Status)
                      .HasConversion<int>()
                      .IsRequired();
            });

            modelBuilder.Entity<RedemptionRecord>(entity =>
            {
                entity.ToTable("RedemptionRecords");
                entity.HasKey(r => r.Id);

                entity.Property(r => r.UserId).IsRequired();
                entity.Property(r => r.ProductId).IsRequired();
            });

            modelBuilder.Entity<RedemptionProcess>(entity =>
            {
                entity.ToTable("RedemptionProcesses");
                entity.HasKey(rp => rp.Id);

                entity.Property(rp => rp.RedemptionId).IsRequired();
                entity.Property(rp => rp.PointsUsed).IsRequired();
                entity.Property(rp => rp.Status)
                      .HasConversion<int>()
                      .IsRequired();
            });

            // ===== TRANSACTIONS =====
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transactions");
                entity.HasKey(t => t.Id);

                entity.Property(t => t.UserId).IsRequired();
                entity.Property(t => t.Amount).IsRequired();
                entity.Property(t => t.RewardPointsEarned).IsRequired();
                entity.Property(t => t.Type)
                      .HasConversion<int>()
                      .IsRequired();
                entity.Property(t => t.Status)
                      .HasConversion<int>()
                      .IsRequired();
            });
        }
    }
}
