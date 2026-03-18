using EventFlow.API.Domain;

public class Event
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Capacity { get; set; }
    public ICollection<Booking> Bookings { get; init; } = new List<Booking>();
}