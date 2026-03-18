using EventFlow.API.Domain;
using EventFlow.API.DTOs;
using EventFlow.API.Interfaces.Repositories;
using EventFlow.API.Interfaces.Services;
namespace EventFlow.API.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public async Task<Event> CreateEvent(CreateEventDTO _event)
        {
            if (_event == null)
                throw new ArgumentNullException("event cannot be null");
            if (string.IsNullOrEmpty(_event.Title) ||
                string.IsNullOrEmpty(_event.Description) ||
                _event.StartDate == default ||
                _event.EndDate == default ||
                _event.Capacity <= 0)
                throw new ArgumentException("Please fill in all the required fields");
            var newEvent = new Event
            {
                Title = _event.Title,
                Description = _event.Description,
                StartDate = _event.StartDate,
                EndDate = _event.EndDate,
                Capacity = _event.Capacity
            };
            return await _eventRepository.AddEvent(newEvent);
        }
        public async Task<bool> DeleteEvent(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("id is missing");
            var _event = await _eventRepository.GetEvent(id);
            if (_event == null)
                throw new KeyNotFoundException("Event not found");
            return await _eventRepository.DeleteEvent(id);
        }
        public async Task<List<Event>> GetAllEvents()
        {
            var events = await _eventRepository.GetAllEvents();
            if (!events.Any())
                throw new KeyNotFoundException("No events found");
            return events;
        }
        public async Task<Event> GetEvent(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("id is missing");
            var _event = await _eventRepository.GetEvent(id);
            if (_event == null)
                throw new KeyNotFoundException("Event not found");
            return _event;
        }
        public async Task<Event> UpdateEvent(UpdateEventDTO _event)
        {
            if (_event == null)
                throw new ArgumentNullException("event cannot be null");
            if (string.IsNullOrEmpty(_event.Title) ||
                string.IsNullOrEmpty(_event.Description) ||
                _event.StartDate == default ||
                _event.EndDate == default ||
                _event.Capacity <= 0)
                throw new ArgumentException("Please fill in all the required fields");
            var existing = await _eventRepository.GetEvent(_event.Id);
            if (existing == null)
                throw new KeyNotFoundException("Event not found");
            existing.Title = _event.Title;
            existing.Description = _event.Description;
            existing.StartDate = _event.StartDate;
            existing.EndDate = _event.EndDate;
            existing.Capacity = _event.Capacity;
            return await _eventRepository.UpdateEvent(existing);
        }
    }
}