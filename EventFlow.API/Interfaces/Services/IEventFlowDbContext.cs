using EventFlow.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.API.Interfaces.Services
{
    public interface IEventFlowDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Booking> Bookings { get; }
        DbSet<Event> Events { get; }
    }
}
