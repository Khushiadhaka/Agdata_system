using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Common
{
    // Unit of work abstraction for committing persistence changes.
    public interface IUnitOfWork
    {
        // Persist changes to the underlying store.
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
