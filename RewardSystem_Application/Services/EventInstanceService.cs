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
	// Handles scheduling and lifecycle of event instances.
	public sealed class EventInstanceService : IEventInstanceService
	{
		private readonly IEventInstanceRepository _repo;
		private readonly IEventDefinitionRepository _defRepo;
		private readonly IUnitOfWork _uow;

		public EventInstanceService(
			IEventInstanceRepository repo,
			IEventDefinitionRepository defRepo,
			IUnitOfWork uow)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_defRepo = defRepo ?? throw new ArgumentNullException(nameof(defRepo));
			_uow = uow ?? throw new ArgumentNullException(nameof(uow));
		}

		public async Task<EventInstance> CreateAsync(
			Guid eventDefinitionId,
			DateTime startTime,
			DateTime endTime,
			CancellationToken ct = default)
		{
			if (eventDefinitionId == Guid.Empty)
				throw new ValidationException("EventDefinitionId required.");

			var def = await _defRepo.GetByIdAsync(eventDefinitionId, ct)
					  ?? throw new ValidationException("Event definition not found.");

			if (!def.IsActive)
				throw new BusinessRuleException("Event definition is inactive.");

			var instance = new EventInstance(eventDefinitionId, startTime, endTime);
			await _repo.AddAsync(instance, ct);
			await _uow.SaveChangesAsync(ct);
			return instance;
		}

		public async Task AssignWinnerAsync(
			Guid instanceId,
			Guid winnerUserId,
			int rank,
			CancellationToken ct = default)
		{
			var inst = await _repo.GetByIdAsync(instanceId, ct)
					   ?? throw new InvalidOperationException("Event instance not found.");

			inst.AssignWinner(winnerUserId, rank);
			await _repo.UpdateAsync(inst, ct);
			await _uow.SaveChangesAsync(ct);
		}

		public async Task MarkCompletedAsync(Guid instanceId, CancellationToken ct = default)
		{
			var inst = await _repo.GetByIdAsync(instanceId, ct)
					   ?? throw new InvalidOperationException("Event instance not found.");

			inst.MarkCompleted();
			await _repo.UpdateAsync(inst, ct);
			await _uow.SaveChangesAsync(ct);
		}

		public async Task CancelAsync(Guid instanceId, CancellationToken ct = default)
		{
			var inst = await _repo.GetByIdAsync(instanceId, ct)
					   ?? throw new InvalidOperationException("Event instance not found.");

			inst.Cancel();
			await _repo.UpdateAsync(inst, ct);
			await _uow.SaveChangesAsync(ct);
		}

		public async Task<EventInstance?> GetByIdAsync(Guid id, CancellationToken ct = default)
		{
			if (id == Guid.Empty) return null;
			return await _repo.GetByIdAsync(id, ct);
		}

		public async Task<IReadOnlyList<EventInstance>> ListByDefinitionAsync(
			Guid eventDefinitionId,
			CancellationToken ct = default)
		{
			if (eventDefinitionId == Guid.Empty)
				throw new ValidationException("EventDefinitionId required.");

			var list = await _repo.GetByDefinitionIdAsync(eventDefinitionId, ct);
			return list.ToList();
		}
	}
}
