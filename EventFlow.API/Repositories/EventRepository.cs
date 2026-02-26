using EventFlow.API.Domain;
using EventFlow.API.Infrastructure.DataAcess;
using EventFlow.API.Interfaces.Repositories;
using EventFlow.API.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventFlow.API.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly EventFlowDbContext _context;

        public EventRepository(EventFlowDbContext eventFlowDbContext)
        {
            _context = eventFlowDbContext;
        }
        public async Task<Event> AddEvent(Event _event)
        {
            var result = await _context.Events.AddAsync(_event);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<bool> DeleteEvent(Guid _id)
        {
            var user = await _context.Events.FindAsync(_id);
            if (user == null) return false;

            var deleted = await _context.Events
                .Where(u => u.Id == user.Id)
                .ExecuteDeleteAsync();

            return deleted > 0;
        }

        public async Task<List<Event>> GetAllEvents()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<Event> GetEvent(Guid id)
        {
            return await _context.Events.FindAsync(id);
        }

        public async Task<Event> UpdateEvent(Event _event)
        {

            var builder = _context.Events.Where(u => u.Id == _event.Id);

            await builder.ExecuteUpdateAsync(u => u
                .SetProperty(x => x.Title, _event.Title)
                .SetProperty(x => x.StartDate, _event.StartDate)
                .SetProperty(x => x.EndDate, _event.EndDate)
                .SetProperty(x => x.Capacity, _event.Capacity)
                .SetProperty(x => x.Description, _event.Description)
                );

            return await _context.Events.FindAsync(_event.Id);
        }

    }
}
