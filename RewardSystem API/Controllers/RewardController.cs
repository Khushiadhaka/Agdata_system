using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewardSystem_API.DTOs.Reward;
using RewardSystem_API.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_API.Controllers
{
    /// <summary>
    /// Manages rewards, reward points and reward transactions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RewardController : ControllerBase
    {
        private readonly IRewardApiService _rewards;

        public RewardController(IRewardApiService rewards)
        {
            _rewards = rewards;
        }

        // --------- Reward definitions ---------

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<RewardDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var list = await _rewards.ListAsync(ct);
            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(RewardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var reward = await _rewards.GetByIdAsync(id, ct);
            if (reward is null)
                return NotFound(new { message = "Reward not found." });

            return Ok(reward);
        }

        [HttpPost]
        [ProducesResponseType(typeof(RewardDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] RewardCreateDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _rewards.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(RewardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] RewardUpdateDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _rewards.UpdateAsync(id, dto, ct);
            if (updated is null)
                return NotFound(new { message = "Reward not found." });

            return Ok(updated);
        }

        // --------- Reward points ---------

        [HttpGet("{id:guid}/points")]
        [ProducesResponseType(typeof(RewardPointsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPoints(Guid id, CancellationToken ct)
        {
            var points = await _rewards.GetRewardPointsAsync(id, ct);
            if (points is null)
                return NotFound(new { message = "Reward points not found." });

            return Ok(points);
        }

        [HttpPost("{id:guid}/points")]
        [ProducesResponseType(typeof(RewardPointsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetPoints(
            Guid id,
            [FromBody] RewardPointsCreateDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // if your DTO has RewardId, keep them in sync
            // dto.RewardId = id;  // uncomment if property exists
            var points = await _rewards.SetRewardPointsAsync(dto, ct);
            return Ok(points);
        }

        // --------- Reward transactions ---------

        [HttpGet("user/{userId:guid}/transactions")]
        [ProducesResponseType(typeof(IReadOnlyList<RewardTransactionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserRewardTransactions(Guid userId, CancellationToken ct)
        {
            var list = await _rewards.ListRewardTransactionsByUserAsync(userId, ct);
            return Ok(list);
        }

        [HttpPost("transactions")]
        [ProducesResponseType(typeof(RewardTransactionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRewardTransaction(
            [FromBody] RewardTransactionCreateDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _rewards.CreateRewardTransactionAsync(dto, ct);
            return Ok(created);
        }

        // --------- Top 3 employees ---------

        [HttpGet("top3-employees")]
        [ProducesResponseType(typeof(IReadOnlyList<Top3EmployeeRewardDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTop3Employees(CancellationToken ct)
        {
            var list = await _rewards.GetTop3EmployeesAsync(ct);
            return Ok(list);
        }
    }
}
