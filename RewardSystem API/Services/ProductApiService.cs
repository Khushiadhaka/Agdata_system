using AutoMapper;
using RewardSystem_API.DTOs.Product;
using RewardSystem_Application.Interfaces.Product;


namespace RewardSystem_API.Services
{
    /// <summary>
    /// Contract for product + inventory operations used by ProductController.
    /// </summary>
    public interface IProductApiService
    {
        Task<IReadOnlyList<ProductDto>> ListAsync(
            CancellationToken cancellationToken = default);

        Task<ProductDto?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<ProductDto> CreateAsync(
            ProductCreateDto dto,
            CancellationToken cancellationToken = default);

        Task<ProductDto?> UpdateAsync(
            Guid id,
            ProductUpdateDto dto,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<ProductInventoryDto?> GetInventoryAsync(
            Guid productId,
            CancellationToken cancellationToken = default);

        Task UpdateStockAsync(
            Guid productId,
            int quantityChange,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Stub implementation – hook to Application layer later.
    /// </summary>
    public sealed class ProductApiService : IProductApiService
    {
        public Task<IReadOnlyList<ProductDto>> ListAsync(
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<ProductDto?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<ProductDto> CreateAsync(
            ProductCreateDto dto,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<ProductDto?> UpdateAsync(
            Guid id,
            ProductUpdateDto dto,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<ProductInventoryDto?> GetInventoryAsync(
            Guid productId,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task UpdateStockAsync(
            Guid productId,
            int quantityChange,
            CancellationToken cancellationToken = default)
            => throw new NotImplementedException();
    }
}
