using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewardSystem_API.DTOs.Transaction;
using RewardSystem_API.Services;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_API.Controllers
{
    /// <summary>
    /// Handles business transactions that can award reward points.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionApiService _transactions;

        public TransactionController(ITransactionApiService transactions)
        {
            _transactions = transactions;
        }

        /// <summary>Create a new transaction.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] TransactionCreateDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _transactions.CreateAsync(dto, ct);
            // Only POST is supported by the current ITransactionApiService.
            return Ok(created);
        }
    }
}
