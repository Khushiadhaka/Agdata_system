using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewardSystem_API.DTOs.Redemption;
using RewardSystem_API.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_API.Controllers
{
    /// <summary>
    /// Handles redemption requests and redemption records.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RedemptionController : ControllerBase
    {
        private readonly IRedemptionApiService _redemption;

        public RedemptionController(IRedemptionApiService redemption)
        {
            _redemption = redemption;
        }

        // ----- Requests -----

        [HttpPost("requests")]
        [ProducesResponseType(typeof(RedemptionRequestDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRequest(
            [FromBody] RedemptionRequestCreateDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _redemption.CreateRequestAsync(dto, ct);
            return Ok(created);
        }

        [HttpPut("requests/{id:guid}")]
        [ProducesResponseType(typeof(RedemptionRequestDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRequest(
            Guid id,
            [FromBody] RedemptionRequestUpdateDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _redemption.UpdateRequestAsync(id, dto, ct);
            return Ok(updated);
        }

        [HttpGet("requests/user/{userId:guid}")]
        [ProducesResponseType(typeof(IReadOnlyList<RedemptionRequestDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRequestsForUser(Guid userId, CancellationToken ct)
        {
            var list = await _redemption.ListRequestsByUserAsync(userId, ct);
            return Ok(list);
        }

        // ----- Records -----

        [HttpGet("records/user/{userId:guid}")]
        [ProducesResponseType(typeof(IReadOnlyList<RedemptionRecordDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecordsForUser(Guid userId, CancellationToken ct)
        {
            var list = await _redemption.ListRecordsByUserAsync(userId, ct);
            return Ok(list);
        }
    }
}
