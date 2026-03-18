public interface IEventService
{
    Task<Event> CreateEvent(CreateEventDTO _event);
    Task<Event> GetEvent(Guid id);
    Task<List<Event>> GetAllEvents();
    Task<Event> UpdateEvent(UpdateEventDTO _event);
    Task<bool> DeleteEvent(Guid id);
}