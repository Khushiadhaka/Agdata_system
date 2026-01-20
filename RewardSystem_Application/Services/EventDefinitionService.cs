using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RewardSystem_Application.Common;
using RewardSystem_Application.Interfaces.Event;
using RewardSystem_Application.Repositories;
using Rewardsystem_Domain.Domain.Common;
using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Application.Services
{
	// Manages creation, update, listing and activation/deactivation of event definitions.
	public sealed class EventDefinitionService : IEventDefinitionService
	{
		private readonly IEventDefinitionRepository _repo;
		private readonly IUnitOfWork _uow;

		public EventDefinitionService(IEventDefinitionRepository repo, IUnitOfWork uow)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_uow = uow ?? throw new ArgumentNullException(nameof(uow));
		}

		public async Task<EventDefinition> CreateAsync(
			string name,
			string? description,
			int rewardPoints,
			CancellationToken ct = default)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ValidationException("Name required.");
			if (rewardPoints <= 0)
				throw new ValidationException("RewardPoints must be positive.");

			var def = new EventDefinition(name.Trim(), description?.Trim(), rewardPoints);
			await _repo.AddAsync(def, ct);
			await _uow.SaveChangesAsync(ct);
			return def;
		}

		public async Task<EventDefinition> UpdateAsync(
			Guid id,
			string name,
			string? description,
			int rewardPoints,
			CancellationToken ct = default)
		{
			if (id == Guid.Empty)
				throw new ValidationException("Id required.");

			var def = await _repo.GetByIdAsync(id, ct)
					  ?? throw new InvalidOperationException("Event definition not found.");

			def.Update(name.Trim(), description?.Trim(), rewardPoints);
			await _repo.UpdateAsync(def, ct);
			await _uow.SaveChangesAsync(ct);
			return def;
		}

		public async Task<EventDefinition?> GetByIdAsync(Guid id, CancellationToken ct = default)
		{
			if (id == Guid.Empty) return null;
			return await _repo.GetByIdAsync(id, ct);
		}

		public async Task<IReadOnlyList<EventDefinition>> ListAsync(
			bool includeInactive = false,
			CancellationToken ct = default)
		{
			var list = await _repo.GetAllAsync(ct);
			return includeInactive
				? list
				: list.Where(x => x.IsActive).ToList();
		}

		public async Task DeactivateAsync(Guid id, CancellationToken ct = default)
		{
			if (id == Guid.Empty)
				throw new ValidationException("Id required.");

			var def = await _repo.GetByIdAsync(id, ct)
					  ?? throw new InvalidOperationException("Event definition not found.");

			def.Deactivate();
			await _repo.UpdateAsync(def, ct);
			await _uow.SaveChangesAsync(ct);
		}

		public async Task ActivateAsync(Guid id, CancellationToken ct = default)
		{
			if (id == Guid.Empty)
				throw new ValidationException("Id required.");

			var def = await _repo.GetByIdAsync(id, ct)
					  ?? throw new InvalidOperationException("Event definition not found.");

			def.Activate();
			await _repo.UpdateAsync(def, ct);
			await _uow.SaveChangesAsync(ct);
		}
	}
}
