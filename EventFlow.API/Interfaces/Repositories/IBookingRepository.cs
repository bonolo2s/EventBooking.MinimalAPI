using EventFlow.API.Domain;

namespace EventFlow.API.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking> AddBooking(Booking booking);
        Task<bool> UpdateBooking(Booking booking);
        Task<bool> DeleteBooking(Guid Id);
        Task<Booking> GetBooking(Guid Id);
        Task<List<Booking>> GetAllBookings();
    }
}
