using EventFlow.API.Domain;

public interface IBookingService
{
    Task<Booking> CreateBooking(CreateBookingDTO booking);
    Task<bool> CancelBooking(Guid id);
    Task<List<Booking>> GetAllBookings();
    Task<Booking> GetBooking(Guid id);
}