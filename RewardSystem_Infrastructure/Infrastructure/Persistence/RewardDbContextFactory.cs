using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RewardSystem_Infrastructure.Infrastructure.Persistence;

namespace RewardSystem_Infrastructure.Persistence
{
    public class RewardDbContextFactory : IDesignTimeDbContextFactory<RewardDbContext>
    {
        public RewardDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RewardDbContext>();

           
            var conn = "Server=DESKTOP-96J8AK0\\SQLEXPRESS;" +
                       "Database=RewardSystemDb;" +
                       "Integrated Security=True;" +
                       "Encrypt=True;" +
                       "TrustServerCertificate=True;";

            optionsBuilder.UseSqlServer(conn, sql =>
            {
                sql.MigrationsAssembly(typeof(RewardDbContext).Assembly.FullName);
            });

            return new RewardDbContext(optionsBuilder.Options);
        }
    }
}
