using Rewardsystem_Domain.Domain.Entities.Event;

namespace RewardSystem_Application.Repositories
{
    // Repository for Event
    public interface IEventRepository
    {
        Task<Event?> GetByIdAsync(Guid id);
        Task<IEnumerable<Event>> ListAsync();
        Task AddAsync(Event ev);
        Task UpdateAsync(Event ev);
        Task<IEnumerable<Event>> ListUpcomingAsync(DateTime from);
    }
}
