using EventFlow.API.Domain;
using EventFlow.API.DTOs;
using EventFlow.API.Interfaces.Repositories;
using EventFlow.API.Interfaces.Services;
namespace EventFlow.API.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEventRepository _eventRepository;

        public BookingService(IBookingRepository bookingRepository, IEventRepository eventRepository)
        {
            _bookingRepository = bookingRepository;
            _eventRepository = eventRepository;
        }

        public async Task<Booking> CreateBooking(CreateBookingDTO booking)
        {
            if (booking == null)
                throw new ArgumentNullException("booking cannot be null");
            if (booking.EventId == Guid.Empty)
                throw new ArgumentException("EventId is missing");
            if (booking.Tickets <= 0)
                throw new ArgumentException("Tickets must be greater than zero");

            var _event = await _eventRepository.GetEvent(booking.EventId);

            if (_event == null)
                throw new KeyNotFoundException("Event not found");

            var activeBookings = await _bookingRepository.GetActiveBookingsCount(booking.EventId);

            if (activeBookings + booking.Tickets > _event.Capacity)
                throw new InvalidOperationException("Not enough capacity for this booking");

            var newBooking = new Booking
            {
                EventId = booking.EventId,
                Tickets = booking.Tickets
            };

            return await _bookingRepository.AddBooking(newBooking);
        }

        public async Task<bool> CancelBooking(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("id is missing");

            var booking = await _bookingRepository.GetBooking(id);

            if (booking == null)
                throw new KeyNotFoundException("Booking not found");

            if (booking.Cancelled)
                throw new InvalidOperationException("Booking is already cancelled");

            booking.Cancelled = true;

            return await _bookingRepository.UpdateBooking(booking);
        }

        public async Task<List<Booking>> GetAllBookings()
        {
            var bookings = await _bookingRepository.GetAllBookings();
            if (!bookings.Any())
                throw new KeyNotFoundException("No bookings found");
            return bookings;
        }

        public async Task<Booking> GetBooking(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("id is missing");

            var booking = await _bookingRepository.GetBooking(id);

            if (booking == null)
                throw new KeyNotFoundException("Booking not found");
            return booking;
        }
    }
}