using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travel.Web.Models;
using Travel.Web.Reposistory;

namespace Travel.Web.Services
{
    public interface IFlightBookingService
    {
        Task<Booking> CreateBookingAsync(Booking booking);
       Task<Booking> GetBookingByIdAsync(int bookingId);
       // Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId);
        Task<bool> UpdateBookingAsync(Booking booking);
        Task<bool> DeleteBookingAsync(int bookingId);
    }

    public class FlightBookingService : IFlightBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FlightBookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException(nameof(booking));

            // Validate flight availability
            var flight = await _unitOfWork.Flights.GetByIdAsync(booking.FlightID);
            if (flight == null)
                throw new InvalidOperationException("Flight not found.");

            // Add booking
            await _unitOfWork.Bookings.AddAsync(booking);
            return booking;
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _unitOfWork.Bookings.GetByIdAsync(bookingId);
        }

      /*  public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _bookingRepository.FindAsync(b => b.UserID == userId);
        }*/

        public async Task<bool> UpdateBookingAsync(Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException(nameof(booking));

            var existingBooking = await _unitOfWork.Bookings.GetByIdAsync(booking.BookingID);
            if (existingBooking == null)
                return false;

            // Update booking details
            existingBooking.FlightID = booking.FlightID;
            existingBooking.ReturnFlightID = booking.ReturnFlightID;
            existingBooking.BookingDate = booking.BookingDate;
            existingBooking.NumberOfPassengers = booking.NumberOfPassengers;
            existingBooking.TripType = booking.TripType;
            existingBooking.ClassID = booking.ClassID;

            await _unitOfWork.Bookings.UpdateAsync(existingBooking);
            return true;
        }

        public async Task<bool> DeleteBookingAsync(int bookingId)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
            if (booking == null)
                return false;

            await _unitOfWork.Bookings.DeleteAsync(bookingId);
            return true;
        }
    }
}
