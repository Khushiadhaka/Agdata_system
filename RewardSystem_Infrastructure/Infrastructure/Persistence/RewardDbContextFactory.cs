using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RewardSystem_Infrastructure.Infrastructure.Persistence;

namespace RewardSystem_Infrastructure.Persistence
{
    // Ensures EF Core CLI tools can create DbContext without full application running.
    public sealed class RewardDbContextFactory : IDesignTimeDbContextFactory<RewardDbContext>
    {
        public RewardDbContext CreateDbContext(string[] args)
        {
            // Get the current working directory (root of project when using migrations).
            var basePath = Directory.GetCurrentDirectory();

            // Load configuration (appsettings.json must contain ConnectionStrings section).
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)                          // Use current directory as base path.
                .AddJsonFile("appsettings.json", optional: true) // Load appsettings.json if present.
                .Build();                                       // Build IConfiguration.

            // Read connection string from config → "DefaultConnection".
            var connectionString = config.GetConnectionString("DefaultConnection");

            // Fallback if appsettings.json not found or connection missing.
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString =
                    "Data Source=DESKTOP-96J8AK0\\SQLEXPRESS;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
            }

            // Setup DbContextOptions.
            var optionsBuilder = new DbContextOptionsBuilder<RewardDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Create and return DbContext.
            return new RewardDbContext(optionsBuilder.Options);
        }
    }
}
