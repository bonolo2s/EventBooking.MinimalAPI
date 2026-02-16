using EventFlow.API.Domain;
using EventFlow.API.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.API.Infrastructure.DataAcess
{
    public class EventFlowDbContext : DbContext, IEventFlowDbContext
    {
        public EventFlowDbContext(DbContextOptions<EventFlowDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var userId = Guid.NewGuid();
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = userId,
                    Name = "Alice Admin",
                    Email = "alice@example.com",
                    Password = "hashed_password_here",
                    Role = Role.Admin
                }
            );

            var event1Id = Guid.NewGuid();
            var event2Id = Guid.NewGuid();

            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = event1Id,
                    Title = "EF Core Workshop",
                    Description = "Learn EF Core in a day!",
                    StartDate = DateTime.UtcNow.AddDays(5),
                    EndDate = DateTime.UtcNow.AddDays(5).AddHours(3),
                    Capacity = 50
                },
                new Event
                {
                    Id = event2Id,
                    Title = "C# Advanced Patterns",
                    Description = "Deep dive into C#.",
                    StartDate = DateTime.UtcNow.AddDays(7),
                    EndDate = DateTime.UtcNow.AddDays(7).AddHours(4),
                    Capacity = 30
                }
            );

            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = Guid.NewGuid(),
                    EventId = event1Id,
                    UserId = userId,
                    Tickets = 2,
                    Cancelled = false,
                    CreatedAt = DateTime.UtcNow
                },
                new Booking
                {
                    Id = Guid.NewGuid(),
                    EventId = event2Id,
                    UserId = userId,
                    Tickets = 1,
                    Cancelled = false,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
