public class CreateBookingDTO
{
    public Guid EventId { get; set; }
    public int Tickets { get; set; } = 1;
}