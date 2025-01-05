using BookingWebApi.Application.Booking;
using BookingWebApi.Domain.Constants;
using BookingWebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Response;

namespace BookingWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController(IBookingService _bookingService) : ControllerBase
    {
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateBooking(BookingDto bookingDto)
        {
            var result = await _bookingService.CreateBooking(UserHelpers.GetUserId(HttpContext), bookingDto);

            return result.ToResponse();
        }
    }
}
