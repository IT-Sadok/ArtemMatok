using AuditWebApi.Application.BookingAudit;
using Contracts.DTOs.Audit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Response;

namespace AuditWebApi.Controllers
{
    [Route("api/bookingAudits")]
    [ApiController]
    public class BookingAuditsController(IAuditBookingService _bookingAudit) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateAuditBooking(AuditBookingCreateDto bookingDto)
        {
            var result = await _bookingAudit.CreateAuditBooking(bookingDto);

            return result.ToResponse();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveAuditBooking([FromQuery]RemoveAuditBookingDto bookingDto)
        {
            var result = await _bookingAudit.RemoveAuditBookingAsync(bookingDto);

            return result.ToResponse();
        }
    }
}
