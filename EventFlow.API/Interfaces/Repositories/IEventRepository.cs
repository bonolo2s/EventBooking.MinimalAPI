using EventFlow.API.Domain;

namespace EventFlow.API.Interfaces.Repositories
{
    public interface IEventRepository
    {
        Task<Event> AddEvent(Event _event);
        Task<Event> GetEvent(Guid id);
        Task<List<Event>> GetAllEvents();
        Task<Event> UpdateEvent(Event _event);
        Task<bool> DeleteEvent(Guid _id);
    }
}
