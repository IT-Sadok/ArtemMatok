using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingEntity = BookingWebApi.Domain.Entities.Booking;

namespace BookingWebApi.Application.Booking
{
    public interface IBookingRepository
    {
        Task<Result<BookingEntity>> CreateBookingAsync(BookingEntity booking);
        Task<Result<bool>> CancelBooking(int bookingId);
    }
}
