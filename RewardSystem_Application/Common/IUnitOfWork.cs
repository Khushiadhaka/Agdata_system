using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Common
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
