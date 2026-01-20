using Microsoft.EntityFrameworkCore;
using Rewardsystem_Domain.Domain.Entities.Event;
using Rewardsystem_Domain.Domain.Entities.Product;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Entities.Transactions;
using Rewardsystem_Domain.Domain.Entities.User;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence
{
	public class RewardDbContext : DbContext
	{
		public RewardDbContext(DbContextOptions<RewardDbContext> options)
			: base(options)
		{
		}

		public DbSet<User> Users => Set<User>();
		public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
		public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

		public DbSet<Product> Products => Set<Product>();
		public DbSet<ProductInventory> ProductInventories => Set<ProductInventory>();

		public DbSet<EventDefinition> EventDefinitions => Set<EventDefinition>();
		public DbSet<EventInstance> EventInstances => Set<EventInstance>();
		public DbSet<EventRewardRule> EventRewardRules => Set<EventRewardRule>();

		public DbSet<Reward> Rewards => Set<Reward>();
		public DbSet<RewardPoints> RewardPoints => Set<RewardPoints>();
		public DbSet<RewardTransaction> RewardTransactions => Set<RewardTransaction>();
		public DbSet<PointsTransaction> PointsTransactions => Set<PointsTransaction>();

		public DbSet<RedemptionRequest> RedemptionRequests => Set<RedemptionRequest>();
		public DbSet<RedemptionProcess> RedemptionProcesses => Set<RedemptionProcess>();
		public DbSet<RedemptionRecord> RedemptionRecords => Set<RedemptionRecord>();

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

					email.HasIndex(e => e.Value).IsUnique();
				});

				entity.OwnsOne(x => x.EmployeeId, emp =>
				{
					emp.Property(e => e.Value)
					   .HasColumnName("EmployeeId")
					   .IsRequired()
					   .HasMaxLength(50);

					emp.HasIndex(e => e.Value).IsUnique();
				});

				entity.Property(x => x.IsDeleted).IsRequired();
				entity.Property(x => x.CreatedAt).IsRequired();

				entity.HasOne(x => x.Account)
					  .WithOne(a => a.User!)
					  .HasForeignKey<UserAccount>(a => a.UserId)
					  .OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(x => x.Profile)
					  .WithOne(p => p.User!)
					  .HasForeignKey<UserProfile>(p => p.UserId)
					  .OnDelete(DeleteBehavior.Cascade);
			});
		}

		// ---------------- PRODUCT ----------------
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

				entity.Property(x => x.RequiredPoints).IsRequired();
				entity.Property(x => x.IsActive).IsRequired();
				entity.Property(x => x.CreatedAt).IsRequired();

				entity.OwnsOne(x => x.Sku, sku =>
				{
					sku.Property(s => s.Value)
					   .HasColumnName("Sku")
					   .IsRequired()
					   .HasMaxLength(50);
				});
			});

			modelBuilder.Entity<ProductInventory>(entity =>
			{
				entity.HasKey(x => x.Id);

				entity.Property(x => x.ProductId).IsRequired();
				entity.Property(x => x.StockQuantity).IsRequired();
				entity.Property(x => x.IsActive).IsRequired();
				entity.Property(x => x.CreatedAt).IsRequired();

				entity.HasOne(pi => pi.Product)
					  .WithMany()
					  .HasForeignKey(pi => pi.ProductId)
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
					  .OnDelete(DeleteBehavior.Restrict);
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

		// ---------------- REWARD + POINTS ----------------
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

			modelBuilder.Entity<PointsTransaction>(entity =>
			{
				entity.HasKey(x => x.Id);

				entity.Property(x => x.UserId).IsRequired();
				entity.Property(x => x.Points).IsRequired();
				entity.Property(x => x.Type).IsRequired();
				entity.Property(x => x.Description).HasMaxLength(500);
				entity.Property(x => x.CreatedAt).IsRequired();

				entity.HasOne<User>()
					  .WithMany()
					  .HasForeignKey(x => x.UserId)
					  .OnDelete(DeleteBehavior.Restrict);
			});
		}

		private static void ConfigureRedemption(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<RedemptionRequest>(entity =>
			{
				entity.HasKey(x => x.Id);

				entity.Property(x => x.UserId).IsRequired();
				entity.Property(x => x.ProductId).IsRequired();
				entity.Property(x => x.PointsUsed).IsRequired();
				entity.Property(x => x.Status).IsRequired();
				entity.Property(x => x.Note).HasMaxLength(500);
				entity.Property(x => x.CreatedAt).IsRequired();

				// ❗ Prevent duplicate pending requests
				entity.HasIndex(x => new { x.UserId, x.ProductId })
					  .HasFilter("[Status] = 0") // Pending
					  .IsUnique();

				entity.HasOne<User>()
					  .WithMany()
					  .HasForeignKey(x => x.UserId)
					  .OnDelete(DeleteBehavior.Restrict);

				entity.HasOne<Product>()
					  .WithMany()
					  .HasForeignKey(x => x.ProductId)
					  .OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<RedemptionProcess>(entity =>
			{
				entity.HasKey(x => x.Id);

				entity.Property(x => x.RedemptionId).IsRequired();
				entity.Property(x => x.PointsUsed).IsRequired();
				entity.Property(x => x.Status).IsRequired();
				entity.Property(x => x.Note).HasMaxLength(500);
				entity.Property(x => x.CreatedAt).IsRequired();

				// 🔗 Strong FK link
				entity.HasOne<RedemptionRequest>()
					  .WithOne()
					  .HasForeignKey<RedemptionProcess>(x => x.RedemptionId)
					  .OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<RedemptionRecord>(entity =>
			{
				entity.HasKey(x => x.Id);

				entity.Property(x => x.UserId).IsRequired();
				entity.Property(x => x.ProductId).IsRequired();
				entity.Property(x => x.Reference).HasMaxLength(200);
				entity.Property(x => x.RedeemedAt).IsRequired();
				entity.Property(x => x.CreatedAt).IsRequired();

				entity.HasOne<User>()
					  .WithMany()
					  .HasForeignKey(x => x.UserId)
					  .OnDelete(DeleteBehavior.Restrict);

				entity.HasOne<Product>()
					  .WithMany()
					  .HasForeignKey(x => x.ProductId)
					  .OnDelete(DeleteBehavior.Restrict);
			});
		}

		// ---------------- TRANSACTION ----------------
		private static void ConfigureTransaction(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Transaction>(entity =>
			{
				entity.HasKey(x => x.Id);

				entity.Property(x => x.UserId).IsRequired();
				entity.Property(x => x.Amount).HasPrecision(18, 4).IsRequired();
				entity.Property(x => x.RewardPointsEarned).IsRequired();
				entity.Property(x => x.Type).IsRequired();
				entity.Property(x => x.Status).IsRequired();
				entity.Property(x => x.Date).IsRequired();
				entity.Property(x => x.CreatedAt).IsRequired();
			});
		}
	}
}
