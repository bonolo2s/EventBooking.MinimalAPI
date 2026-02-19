using EventFlow.API.Domain;

namespace EventFlow.API.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking> AddBooking(Booking booking);
        Task<bool> UpdateBooking(Booking booking);
        Task<bool> DeleteBooking(string Id);
        Task<Booking> GetBooking(string Id);
        Task<List<Booking>> GetAllBookings();
    }
}
