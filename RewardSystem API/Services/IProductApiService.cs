using RewardSystem_API.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_API.Services
{
	public interface IProductApiService
	{
		Task<IReadOnlyList<ProductDto>> ListAsync(
			CancellationToken ct = default);

		Task<ProductDto?> GetByIdAsync(
			Guid id,
			CancellationToken ct = default);

		Task<ProductDto> CreateAsync(
			ProductCreateDto dto,
			CancellationToken ct = default);

		Task<ProductDto> UpdateAsync(
			Guid id,
			ProductUpdateDto dto,
			CancellationToken ct = default);

		Task DeactivateAsync(
			Guid id,
			CancellationToken ct = default);

		Task AdjustStockAsync(
			Guid id,
			int delta,
			CancellationToken ct = default);
	}
}

