using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using RewardSystem_API.DTOs.Product;
using RewardSystem_Application.Interfaces.Inventory;
using RewardSystem_Application.Interfaces.Product;

namespace RewardSystem_API.Services
{
	public sealed class ProductApiService : IProductApiService
	{
		private readonly IProductService _productService;
		private readonly IInventoryService _inventoryService;
		private readonly IMapper _mapper;

		public ProductApiService(
			IProductService productService,
			IInventoryService inventoryService,
			IMapper mapper)
		{
			_productService = productService;
			_inventoryService = inventoryService;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<ProductDto>> ListAsync(CancellationToken ct = default)
		{
			var products = await _productService.ListAsync(false, ct);
			return _mapper.Map<IReadOnlyList<ProductDto>>(products);
		}

		public async Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
		{
			var product = await _productService.GetByIdAsync(id, ct);
			return product == null ? null : _mapper.Map<ProductDto>(product);
		}

		public async Task<ProductDto> CreateAsync(ProductCreateDto dto, CancellationToken ct = default)
		{
			var product = await _productService.CreateProductAsync(
				dto.Name,
				dto.Description,
				dto.RequiredPoints,
				dto.InitialStock,
				dto.SKU,
				ct);

			return _mapper.Map<ProductDto>(product);
		}

		public async Task<ProductDto> UpdateAsync(Guid id, ProductUpdateDto dto, CancellationToken ct = default)
		{
			var product = await _productService.UpdateProductAsync(
				id,
				dto.Name,
				dto.Description,
				dto.RequiredPoints,
				dto.SKU,
				ct);

			return _mapper.Map<ProductDto>(product);
		}

		public async Task DeactivateAsync(Guid id, CancellationToken ct = default)
		{
			await _productService.DeactivateAsync(id, ct);
		}

		public async Task AdjustStockAsync(Guid id, int delta, CancellationToken ct = default)
		{
			await _productService.AdjustStockAsync(id, delta, ct);
		}
	}
}
