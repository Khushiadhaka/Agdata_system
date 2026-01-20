using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewardSystem_API.DTOs.Product;
using RewardSystem_API.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_API.Controllers
{
	/// <summary>
	/// Manages products that can be redeemed with reward points.
	/// </summary>
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class ProductController : ControllerBase
	{
		private readonly IProductApiService _products;

		public ProductController(IProductApiService products)
		{
			_products = products;
		}

		// ---------- CATALOG ----------

		[HttpGet]
		[ProducesResponseType(typeof(IReadOnlyList<ProductDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll(CancellationToken ct)
		{
			var list = await _products.ListAsync(ct);
			return Ok(list);
		}

		[HttpGet("{id:guid}")]
		[ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
		{
			var product = await _products.GetByIdAsync(id, ct);
			if (product is null)
				return NotFound(new { message = "Product not found." });

			return Ok(product);
		}

		// ---------- ADMIN ----------

		[HttpPost]
		[ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create(
			[FromBody] ProductCreateDto dto,
			CancellationToken ct)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var created = await _products.CreateAsync(dto, ct);
			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}

		[HttpPut("{id:guid}")]
		[ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Update(
			Guid id,
			[FromBody] ProductUpdateDto dto,
			CancellationToken ct)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var updated = await _products.UpdateAsync(id, dto, ct);
			return Ok(updated);
		}

		/// <summary>
		/// Soft-deactivate a product (cannot deactivate if pending redemptions exist).
		/// </summary>
		[HttpDelete("{id:guid}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
		{
			await _products.DeactivateAsync(id, ct);
			return NoContent();
		}

		/// <summary>
		/// Adjust product stock (positive = add, negative = reduce).
		/// </summary>
		[HttpPost("{id:guid}/stock")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> AdjustStock(
			Guid id,
			[FromQuery] int delta,
			CancellationToken ct)
		{
			await _products.AdjustStockAsync(id, delta, ct);
			return NoContent();
		}
	}
}
