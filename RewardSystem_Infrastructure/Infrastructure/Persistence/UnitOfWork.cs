using RewardSystem_Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Infrastructure.Infrastructure.Persistence
{
    // Implements IUnitOfWork using EF Core DbContext
    public sealed class UnitOfWork : IUnitOfWork
    {
        // EF Core DbContext instance
        private readonly RewardDbContext _dbContext;

        // Inject DbContext via constructor
        public UnitOfWork(RewardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Save all pending changes to the database
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

    }
}
