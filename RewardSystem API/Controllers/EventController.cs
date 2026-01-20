using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewardSystem_API.DTOs.Event;
using RewardSystem_API.Services;


using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RewardSystem_API.Controllers
{
	/// <summary>
	/// Manages event definitions, instances and reward rules.
	/// </summary>
	[ApiController]
	[Route("api/events")]
	[Authorize]
	public class EventController : ControllerBase
	{
		private readonly IEventApiService _events;

		public EventController(IEventApiService events)
		{
			_events = events;
		}

		// ================= EVENT DEFINITIONS =================

		// Anyone can view definitions
		[HttpGet("definitions")]
		[ProducesResponseType(typeof(IReadOnlyList<EventDefinitionDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetDefinitions(CancellationToken ct)
		{
			var list = await _events.ListDefinitionsAsync(ct);
			return Ok(list);
		}

		[HttpGet("definitions/{id:guid}")]
		[ProducesResponseType(typeof(EventDefinitionDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetDefinitionById(Guid id, CancellationToken ct)
		{
			var def = await _events.GetDefinitionByIdAsync(id, ct);
			if (def is null)
				return NotFound(new { message = "Event definition not found." });

			return Ok(def);
		}

		// Admin only
		[HttpPost("definitions")]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(typeof(EventDefinitionDto), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateDefinition(
			[FromBody] EventDefinitionCreateDto dto,
			CancellationToken ct)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var created = await _events.CreateDefinitionAsync(dto, ct);
			return CreatedAtAction(nameof(GetDefinitionById), new { id = created.Id }, created);
		}

		// Admin only
		[HttpPut("definitions/{id:guid}")]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(typeof(EventDefinitionDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> UpdateDefinition(
			Guid id,
			[FromBody] EventDefinitionUpdateDto dto,
			CancellationToken ct)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var updated = await _events.UpdateDefinitionAsync(id, dto, ct);
			if (updated is null)
				return NotFound(new { message = "Event definition not found." });

			return Ok(updated);
		}

		// ================= EVENT INSTANCES =================

		[HttpGet("instances")]
		[ProducesResponseType(typeof(IReadOnlyList<EventInstanceDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetInstances(CancellationToken ct)
		{
			var list = await _events.ListInstancesAsync(ct);
			return Ok(list);
		}

		// Admin only
		[HttpPost("instances")]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(typeof(EventInstanceDto), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateInstance(
			[FromBody] EventInstanceCreateDto dto,
			CancellationToken ct)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var created = await _events.CreateInstanceAsync(dto, ct);
			return CreatedAtAction(nameof(GetInstances), new { id = created.Id }, created);
		}

		// ================= REWARD RULES =================

		[HttpGet("definitions/{id:guid}/rules")]
		[ProducesResponseType(typeof(IReadOnlyList<EventRewardRuleDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetRules(Guid id, CancellationToken ct)
		{
			var list = await _events.ListRewardRulesAsync(id, ct);
			return Ok(list);
		}

		// Admin only
		[HttpPost("rules")]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(typeof(EventRewardRuleDto), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateRule(
			[FromBody] EventRewardRuleCreateDto dto,
			CancellationToken ct)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var created = await _events.CreateRewardRuleAsync(dto, ct);
			return CreatedAtAction(nameof(GetRules), new { id = created.EventDefinitionId }, created);
		}
	}
}
