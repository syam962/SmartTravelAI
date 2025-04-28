using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travel.Web.Models;
using Travel.Web.Models.DTOs;
using Travel.Web.Reposistory;

namespace Travel.Web.Services
{
    public interface IFlightBookingService
    {
        Task CreateBookingAsync(int segmentId, int passengerCount);
        Task<Booking> GetBookingByIdAsync(int bookingId);

        Task<bool> UpdateBookingAsync(Booking booking);
        Task<bool> DeleteBookingAsync(int bookingId);
        Task<IEnumerable<BookingDTO>> GetBookingsByUserIdAsync(int userId);
    }

    public class FlightBookingService : IFlightBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FlightBookingService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task CreateBookingAsync(int segmentId, int passengerCount = 1)
        {
            // Retrieve the flight segment details
            var flightSegment = await _unitOfWork.FlightSegments.GetByIdAsync(segmentId);
            if (flightSegment == null)
                throw new InvalidOperationException("Flight segment not found.");



            Booking booking = new Booking
            {
                FlightID = flightSegment.FlightID,

                BookingDate = DateTime.UtcNow,
                NumberOfPassengers = passengerCount,
                ClassID = 1,// Default to 1 passenger
                TripType = "OneWay", // Default to OneWay
                UserID = 1

            };
            BookingSegment segment = new BookingSegment
            {
                SegmentID = segmentId,
                PassengerCount = passengerCount,
                ClassID = 1,


            };
            await _unitOfWork.Bookings.AddBookingAsync(booking, new List<BookingSegment> { segment });


        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _unitOfWork.Bookings.GetByIdAsync(bookingId);
        }



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

        public async Task<IEnumerable<BookingDTO>> GetBookingsByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID.", nameof(userId));
            var bookings = await _unitOfWork.Bookings.GetBookingsByUserIdAsync(userId);
            IEnumerable<BookingDTO> lstBookings = _mapper.Map<IEnumerable<BookingDTO>>(bookings);
            return lstBookings;
        }


    }
}
