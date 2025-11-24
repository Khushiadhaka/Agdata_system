using Microsoft.AspNetCore.Mvc;
using RewardSystem_Application.Services.Interfaces;
using Rewardsystem_Domain.Domain.Entities.User;
using Rewardsystem_Domain.Domain.Entities.Product;
using Rewardsystem_Domain.Domain.Entities.Reward;
using Rewardsystem_Domain.Domain.Entities.Event;
using Rewardsystem_Domain.Domain.Entities.Redemption;
using Rewardsystem_Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    // ====================== REQUEST MODELS (DTOs for this file) ======================

    // --- User ---
    public class CreateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }

    public class UpdateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }

    // --- Product ---
    public class CreateProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RequiredPoints { get; set; }
    }

    public class UpdateProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RequiredPoints { get; set; }
    }

    public class ChangeStockRequest
    {
        public int Quantity { get; set; }
    }

    // --- Reward ---
    public class CreateRewardRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public RewardType Type { get; set; }
    }

    public class UpdateRewardRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public RewardType Type { get; set; }
    }

    public class ConfigureRewardPointsRequest
    {
        public int Points { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }

    // --- Event ---
    public class CreateEventDefinitionRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RewardPoints { get; set; }
    }

    public class ScheduleEventInstanceRequest
    {
        public Guid EventDefinitionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class AssignWinnerRequest
    {
        public Guid WinnerUserId { get; set; }
        public int Rank { get; set; }
    }

    // --- Redemption ---
    public class CreateRedemptionRequestModel
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int PointsUsed { get; set; }
    }

    // ====================== USERS CONTROLLER ======================

    [ApiController]
    [Route("api/[controller]")]  // /api/users
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /api/users
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<User>>> GetAll(CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllAsync(cancellationToken);
            return Ok(users);
        }

        // GET: /api/users/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<User>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);
            if (user is null)
                return NotFound();

            return Ok(user);
        }

        // POST: /api/users
        [HttpPost]
        public async Task<ActionResult<User>> Create(
            [FromBody] CreateUserRequest request,
            CancellationToken cancellationToken)
        {
            var user = await _userService.CreateUserAsync(
                request.Name,
                request.Email,
                request.EmployeeId,
                request.Role,
                cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // PUT: /api/users/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateUserRequest request,
            CancellationToken cancellationToken)
        {
            await _userService.UpdateUserAsync(
                id,
                request.Name,
                request.Email,
                request.Role,
                cancellationToken);

            return NoContent();
        }

        // DELETE: /api/users/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _userService.DeleteUserAsync(id, cancellationToken);
            return NoContent();
        }

        // POST: /api/users/{id}/account  -> create UserAccount
        [HttpPost("{id:guid}/account")]
        public async Task<ActionResult<UserAccount>> CreateAccount(Guid id, CancellationToken cancellationToken)
        {
            var account = await _userService.CreateUserAccountAsync(id, cancellationToken);
            return Ok(account);
        }
    }

    // ====================== PRODUCTS CONTROLLER ======================

    [ApiController]
    [Route("api/[controller]")]  // /api/products
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: /api/products
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetAll(CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllAsync(cancellationToken);
            return Ok(products);
        }

        // GET: /api/products/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Product>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdAsync(id, cancellationToken);
            if (product is null)
                return NotFound();

            return Ok(product);
        }

        // POST: /api/products
        [HttpPost]
        public async Task<ActionResult<Product>> Create(
            [FromBody] CreateProductRequest request,
            CancellationToken cancellationToken)
        {
            var product = await _productService.CreateProductAsync(
                request.Name,
                request.Description,
                request.RequiredPoints,
                cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // PUT: /api/products/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateProductRequest request,
            CancellationToken cancellationToken)
        {
            await _productService.UpdateProductAsync(
                id,
                request.Name,
                request.Description,
                request.RequiredPoints,
                cancellationToken);

            return NoContent();
        }

        // PATCH: /api/products/{id}/deactivate
        [HttpPatch("{id:guid}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
        {
            await _productService.DeactivateProductAsync(id, cancellationToken);
            return NoContent();
        }

        // POST: /api/products/{id}/inventory/initial
        [HttpPost("{id:guid}/inventory/initial")]
        public async Task<ActionResult<ProductInventory>> SetInitialInventory(
            Guid id,
            [FromBody] ChangeStockRequest request,
            CancellationToken cancellationToken)
        {
            var inventory = await _productService.SetInitialInventoryAsync(
                id,
                request.Quantity,
                cancellationToken);

            return Ok(inventory);
        }

        // POST: /api/products/{id}/inventory/increase
        [HttpPost("{id:guid}/inventory/increase")]
        public async Task<IActionResult> IncreaseStock(
            Guid id,
            [FromBody] ChangeStockRequest request,
            CancellationToken cancellationToken)
        {
            await _productService.IncreaseStockAsync(id, request.Quantity, cancellationToken);
            return NoContent();
        }

        // POST: /api/products/{id}/inventory/reduce
        [HttpPost("{id:guid}/inventory/reduce")]
        public async Task<IActionResult> ReduceStock(
            Guid id,
            [FromBody] ChangeStockRequest request,
            CancellationToken cancellationToken)
        {
            await _productService.ReduceStockAsync(id, request.Quantity, cancellationToken);
            return NoContent();
        }
    }

    // ====================== REWARDS CONTROLLER ======================

    [ApiController]
    [Route("api/[controller]")]  // /api/rewards
    public class RewardsController : ControllerBase
    {
        private readonly IRewardService _rewardService;

        public RewardsController(IRewardService rewardService)
        {
            _rewardService = rewardService;
        }

        // GET: /api/rewards
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Reward>>> GetAll(CancellationToken cancellationToken)
        {
            var rewards = await _rewardService.GetAllRewardsAsync(cancellationToken);
            return Ok(rewards);
        }

        // GET: /api/rewards/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Reward>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var reward = await _rewardService.GetRewardByIdAsync(id, cancellationToken);
            if (reward is null)
                return NotFound();

            return Ok(reward);
        }

        // POST: /api/rewards
        [HttpPost]
        public async Task<ActionResult<Reward>> Create(
            [FromBody] CreateRewardRequest request,
            CancellationToken cancellationToken)
        {
            var reward = await _rewardService.CreateRewardAsync(
                request.Name,
                request.Description,
                request.Type,
                cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = reward.Id }, reward);
        }

        // PUT: /api/rewards/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateRewardRequest request,
            CancellationToken cancellationToken)
        {
            await _rewardService.UpdateRewardAsync(
                id,
                request.Name,
                request.Description,
                request.Type,
                cancellationToken);

            return NoContent();
        }

        // PATCH: /api/rewards/{id}/deactivate
        [HttpPatch("{id:guid}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
        {
            await _rewardService.DeactivateRewardAsync(id, cancellationToken);
            return NoContent();
        }

        // PUT: /api/rewards/{id}/points
        [HttpPut("{id:guid}/points")]
        public async Task<ActionResult<RewardPoints>> ConfigurePoints(
            Guid id,
            [FromBody] ConfigureRewardPointsRequest request,
            CancellationToken cancellationToken)
        {
            var points = await _rewardService.ConfigureRewardPointsAsync(
                id,
                request.Points,
                request.EffectiveFrom,
                request.EffectiveTo,
                cancellationToken);

            return Ok(points);
        }
    }

    // ====================== EVENTS CONTROLLER ======================

    [ApiController]
    [Route("api/[controller]")]  // /api/events
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // GET: /api/events/definitions
        [HttpGet("definitions")]
        public async Task<ActionResult<IReadOnlyList<EventDefinition>>> GetDefinitions(
            CancellationToken cancellationToken)
        {
            var defs = await _eventService.GetAllEventDefinitionsAsync(cancellationToken);
            return Ok(defs);
        }

        // GET: /api/events/definitions/{id}
        [HttpGet("definitions/{id:guid}")]
        public async Task<ActionResult<EventDefinition>> GetDefinitionById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var def = await _eventService.GetEventDefinitionByIdAsync(id, cancellationToken);
            if (def is null)
                return NotFound();

            return Ok(def);
        }

        // POST: /api/events/definitions
        [HttpPost("definitions")]
        public async Task<ActionResult<EventDefinition>> CreateDefinition(
            [FromBody] CreateEventDefinitionRequest request,
            CancellationToken cancellationToken)
        {
            var def = await _eventService.CreateEventDefinitionAsync(
                request.Name,
                request.Description,
                request.RewardPoints,
                cancellationToken);

            return CreatedAtAction(nameof(GetDefinitionById), new { id = def.Id }, def);
        }

        // POST: /api/events/instances
        [HttpPost("instances")]
        public async Task<ActionResult<EventInstance>> ScheduleInstance(
            [FromBody] ScheduleEventInstanceRequest request,
            CancellationToken cancellationToken)
        {
            var instance = await _eventService.ScheduleEventInstanceAsync(
                request.EventDefinitionId,
                request.StartTime,
                request.EndTime,
                cancellationToken);

            return Ok(instance);
        }

        // POST: /api/events/instances/{id}/winner
        [HttpPost("instances/{id:guid}/winner")]
        public async Task<IActionResult> AssignWinner(
            Guid id,
            [FromBody] AssignWinnerRequest request,
            CancellationToken cancellationToken)
        {
            await _eventService.AssignWinnerAsync(
                id,
                request.WinnerUserId,
                request.Rank,
                cancellationToken);

            return NoContent();
        }
    }

    // ====================== REDEMPTIONS CONTROLLER ======================

    [ApiController]
    [Route("api/[controller]")]  // /api/redemptions
    public class RedemptionsController : ControllerBase
    {
        private readonly IRedemptionService _redemptionService;

        public RedemptionsController(IRedemptionService redemptionService)
        {
            _redemptionService = redemptionService;
        }

        // POST: /api/redemptions/requests
        [HttpPost("requests")]
        public async Task<ActionResult<RedemptionRequest>> CreateRequest(
            [FromBody] CreateRedemptionRequestModel request,
            CancellationToken cancellationToken)
        {
            var result = await _redemptionService.CreateRedemptionRequestAsync(
                request.UserId,
                request.ProductId,
                request.PointsUsed,
                cancellationToken);

            return Ok(result);
        }

        // PATCH: /api/redemptions/requests/{id}/approve
        [HttpPatch("requests/{id:guid}/approve")]
        public async Task<IActionResult> Approve(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _redemptionService.ApproveRedemptionAsync(id, cancellationToken);
            return NoContent();
        }

        // PATCH: /api/redemptions/requests/{id}/reject
        [HttpPatch("requests/{id:guid}/reject")]
        public async Task<IActionResult> Reject(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _redemptionService.RejectRedemptionAsync(id, cancellationToken);
            return NoContent();
        }

        // PATCH: /api/redemptions/requests/{id}/complete
        [HttpPatch("requests/{id:guid}/complete")]
        public async Task<IActionResult> Complete(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _redemptionService.CompleteRedemptionAsync(id, cancellationToken);
            return NoContent();
        }

        // GET: /api/redemptions/users/{userId}?status=Pending
        [HttpGet("users/{userId:guid}")]
        public async Task<ActionResult<IReadOnlyList<RedemptionRequest>>> GetForUser(
            Guid userId,
            [FromQuery] RedemptionStatus status,
            CancellationToken cancellationToken)
        {
            var result = await _redemptionService.GetUserRequestsByStatusAsync(
                userId,
                status,
                cancellationToken);

            return Ok(result);
        }
    }
}
