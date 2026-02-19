using EventFlow.API.Domain;
using EventFlow.API.Infrastructure.DataAcess;
using EventFlow.API.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.API.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly EventFlowDbContext _context;

        public BookingRepository(EventFlowDbContext eventFlowDbContext)
        {
            _context = eventFlowDbContext;
        }

        public async Task<Booking> AddBooking(Booking booking)
        {
            var result = await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<bool> DeleteBooking(string Id)
        {
            var booking = await _context.Bookings.FindAsync(Id);
            var builder = await _context.Bookings.Where(u => u.Id == booking.Id)
                .ExecuteDeleteAsync();

            return builder > 0;
        }

        public async Task<List<Booking>> GetAllBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        public async Task<Booking> GetBooking(string Id)
        {
            return await _context.Bookings.FindAsync(Id);
        }

        //admin only
        public async Task<bool> UpdateBooking(Booking booking)
        {
            var builder =await _context.Bookings.Where(u => u.Id == booking.Id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(x => x.Tickets, booking.Tickets)
                .SetProperty(x => x.Cancelled, booking.Cancelled)
                );

            return builder > 0;
        }
    }
}
