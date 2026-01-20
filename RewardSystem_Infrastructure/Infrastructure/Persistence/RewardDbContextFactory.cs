using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RewardSystem_Infrastructure.Infrastructure.Persistence;

namespace RewardSystem_Infrastructure.Persistence
{
	// Ensures EF Core CLI tools can create DbContext without full application running.
	public sealed class RewardDbContextFactory
		: IDesignTimeDbContextFactory<RewardDbContext>
	{
		public RewardDbContext CreateDbContext(string[] args)
		{
			// Project root directory
			var basePath = Directory.GetCurrentDirectory();

			// Load appsettings.json
			var config = new ConfigurationBuilder()
				.SetBasePath(basePath)
				.AddJsonFile("appsettings.json", optional: true)
				.Build();

			// Try to read connection string
			var connectionString = config.GetConnectionString("DefaultConnection");

			// Fallback connection string (IMPORTANT FIX)
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				connectionString =
					"Server=INDLAP-KHUSHIAD\\SQLEXPRESS;" +
					"Database=RewardSystemDb;" +
					"Trusted_Connection=True;" +
					"TrustServerCertificate=True;";
			}

			// DbContext options
			var optionsBuilder = new DbContextOptionsBuilder<RewardDbContext>();
			optionsBuilder.UseSqlServer(connectionString);

			return new RewardDbContext(optionsBuilder.Options);
		}
	}
}
