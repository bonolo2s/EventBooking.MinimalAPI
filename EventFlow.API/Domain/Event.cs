namespace EventFlow.API.Domain
{
    public class Event
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public int Capacity { get; init; }
        public ICollection<Booking> Bookings { get; init; } = new List<Booking>();
    }
}
