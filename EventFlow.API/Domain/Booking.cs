namespace EventFlow.API.Domain
{
    public class Booking
    {
        public Guid Id { get; init; }
        public Guid EventId { get; init; }
        public Event Event { get; init; } = null!;
        public Guid UserId { get; init; }
        public User User { get; init; } = null!;
        public int Tickets { get; init; } = 1;// quantity reserved for now
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public bool Cancelled { get; set; } = false;

    }
}
