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

            // static IDs
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var event1Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var event2Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var booking1Id = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var booking2Id = Guid.Parse("55555555-5555-5555-5555-555555555555");

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = userId,
                    FullName = "Alice Admin",
                    Email = "alice@example.com",
                    Password = "hashed_password_here",
                    Role = Role.Admin
                }
            );

            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = event1Id,
                    Title = "EF Core Workshop",
                    Description = "Learn EF Core in a day!",
                    StartDate = new DateTime(2026, 2, 20, 9, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 2, 20, 12, 0, 0, DateTimeKind.Utc),
                    Capacity = 50
                },
                new Event
                {
                    Id = event2Id,
                    Title = "C# Advanced Patterns",
                    Description = "Deep dive into C#.",
                    StartDate = new DateTime(2026, 2, 22, 10, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 2, 22, 14, 0, 0, DateTimeKind.Utc),
                    Capacity = 30
                }
            );

            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = booking1Id,
                    EventId = event1Id,
                    UserId = userId,
                    Tickets = 2,
                    Cancelled = false,
                    CreatedAt = new DateTime(2026, 2, 15, 12, 0, 0, DateTimeKind.Utc)
                },
                new Booking
                {
                    Id = booking2Id,
                    EventId = event2Id,
                    UserId = userId,
                    Tickets = 1,
                    Cancelled = false,
                    CreatedAt = new DateTime(2026, 2, 15, 13, 0, 0, DateTimeKind.Utc)
                }
            );
        }

    }
}
