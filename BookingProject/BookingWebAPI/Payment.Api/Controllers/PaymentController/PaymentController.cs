using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.Interfaces.PaymentInterface;

namespace Payment.Api.Controllers.Balance
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPaymentService _paymentService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateBalance([FromQuery] string userId)
        {
            var result = await _paymentService.CreateBalanceAsync(userId);
            
            if(result == false)
            {
                return BadRequest("Something went wrong");
            }

            return Ok(result);
        }

    }
}
