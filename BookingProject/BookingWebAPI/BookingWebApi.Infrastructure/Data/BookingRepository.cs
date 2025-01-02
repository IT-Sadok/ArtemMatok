using BookingWebApi.Application.Booking;
using BookingWebApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Infrastructure.Data
{
    public class BookingRepository(ApplicationDbContext _context): IBookingRepository
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public async Task<Result<bool>> CancelBooking(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            if (booking == null) return Result<bool>.Failure("Booking wasn`t found");

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<Booking>> CreateBookingAsync(Booking booking)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (booking.StartDate >= booking.EndDate)
                {
                    return Result<Booking>.Failure("StartDate must be earlier than EndDate.");
                }

                var isDateAvailable = !await _context.Bookings
                    .AnyAsync(x =>
                        x.ApartamentId == booking.ApartamentId &&
                        !(booking.EndDate <= x.StartDate || booking.StartDate >= x.EndDate)); 

                if(!isDateAvailable)
                {
                    return Result<Booking>.Failure("The selected dates are already booked");
                }

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                return Result<Booking>.Success(booking);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
