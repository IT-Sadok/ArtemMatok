using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.DTOs;
using Payment.Application.Interfaces.PaymentInterface;
using Response;

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

        [HttpPatch("ReserveBalance")]
        public async Task<IActionResult> ReserveBalance(BalanceRequestDto balanceDto)
        {
            var result = await _paymentService.ReserveBalance(balanceDto);

            return result.ToResponse();
        }

        [HttpPatch("CompensateBalance")]
        public async Task<IActionResult> CompensateBalance(BalanceRequestDto balanceDto)
        {
            var result = await _paymentService.CompensateBalance(balanceDto);

            return result.ToResponse();
        }
    }
}
