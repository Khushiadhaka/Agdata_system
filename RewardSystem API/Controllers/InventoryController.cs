using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewardSystem_API.DTOs.Product;
using RewardSystem_API.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_API.Controllers
{
    /// <summary>
    /// Handles stock / inventory for redeemable products.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryApiService _inventory;

        public InventoryController(IInventoryApiService inventory)
        {
            _inventory = inventory;
        }

        [HttpGet("{productId:guid}")]
        [ProducesResponseType(typeof(ProductInventoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByProduct(Guid productId, CancellationToken ct)
        {
            var inv = await _inventory.GetInventoryAsync(productId, ct);
            if (inv is null)
                return NotFound(new { message = "Inventory record not found." });

            return Ok(inv);
        }

        public class UpdateStockRequest
        {
            public int QuantityChange { get; set; }
        }

        [HttpPost("{productId:guid}/update-stock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStock(
            Guid productId,
            [FromBody] UpdateStockRequest request,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _inventory.UpdateStockAsync(productId, request.QuantityChange, ct);
            if (!success)
                return BadRequest(new { message = "Could not update stock." });

            return Ok(new { message = "Stock updated." });
        }
    }
}
