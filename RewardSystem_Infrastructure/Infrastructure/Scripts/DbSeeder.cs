using Microsoft.EntityFrameworkCore;
using Rewardsystem_Domain.Domain.Entities.Product;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Enums;
using RewardSystem_Infrastructure.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Infrastructure.Infrastructure.Scripts
{
    // Seeds initial demo data into the database 
    public static class DbSeeder
    {
        // Entry method to run seeding logic.
        public static async Task SeedAsync(RewardDbContext db, CancellationToken ct = default)
        {
            // Apply pending migrations (sync version to avoid MigrateAsync error)
            db.Database.Migrate();

            // --- Users ---
            if (!db.Users.Any())
            {
                await SeedUsersAsync(db, ct);
            }

            // --- Products + Inventory ---
            if (!db.Products.Any())
            {
                await SeedProductsAsync(db, ct);
            }

            // --- Rewards + RewardPoints ---
            if (!db.Rewards.Any())
            {
                await SeedRewardsAsync(db, ct);
            }

            await db.SaveChangesAsync(ct);
        }

        // Create some default users + their accounts / profiles.
        private static async Task SeedUsersAsync(RewardDbContext db, CancellationToken ct)
        {
            var users = new List<User>
            {
                new User("Admin User", "admin@example.com",    "EMP001", UserRole.Admin),
                new User("John Doe",   "john.doe@example.com", "EMP002", UserRole.User),
                new User("Jane Doe",   "jane.doe@example.com", "EMP003", UserRole.User)
            };

            foreach (var user in users)
            {
                // User
                await db.Users.AddAsync(user, ct);

                // Account with 0 points
                var account = new UserAccount(user.Id);
                await db.UserAccounts.AddAsync(account, ct);

                // Basic profile
                var profile = new UserProfile(
                    userId: user.Id,
                    phoneNumber: "9999999999",
                    department: "IT",
                    location: "HQ");

                await db.UserProfiles.AddAsync(profile, ct);
            }
        }

        // Create some default products with inventory.
        private static async Task SeedProductsAsync(RewardDbContext db, CancellationToken ct)
        {
            var products = new List<Product>
            {
                new Product("Bluetooth Headphones", "Wireless over-ear headphones", 500),
                new Product("Coffee Mug",            "Company branded mug",          150),
                new Product("Backpack",              "Laptop backpack",              400)
            };

            foreach (var product in products)
            {
                await db.Products.AddAsync(product, ct);

                // Give each product some starting stock.
                var inventory = new ProductInventory(product.Id, 50);
                await db.ProductInventories.AddAsync(inventory, ct);
            }
        }

        // Create some default rewards + points.
        private static async Task SeedRewardsAsync(RewardDbContext db, CancellationToken ct)
        {
            // We don't rely on specific enum names (Performance/Project/etc.)
            // to avoid compile errors. We just cast from int.
            var rewards = new List<Reward>
            {
                new Reward("Best Performer of Month",   "Awarded to top performer",       (RewardType)0),
                new Reward("On-Time Project Delivery", "Delivered project on time",     (RewardType)0),
                new Reward("Innovation Award",         "New idea / improvement",        (RewardType)0)
            };

            foreach (var reward in rewards)
            {
                await db.Rewards.AddAsync(reward, ct);

                var points = new RewardPoints(
                    rewardId: reward.Id,
                    points: 300,
                    effectiveFrom: DateTime.UtcNow.Date,
                    effectiveTo: null);

                await db.RewardPoints.AddAsync(points, ct);
            }
        }
    }
}
