using Rewardsystem_Domain.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        // Return only active products
        Task<IReadOnlyList<Product>> GetActiveAsync(
            CancellationToken cancellationToken = default);
    }
}
