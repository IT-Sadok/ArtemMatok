using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.Dtos;
using Payment.Application.Services;

namespace Payment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController(IBalanceService _balanceService) : ControllerBase
    {
        [HttpGet()]
        public async Task<IActionResult> GetBalanceInfo(string userId)
        {
            var result = await _balanceService.GetBalanceInfoAsync(userId);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch]
        public async Task<IActionResult> ChangeBalance([FromBody] ChangeBalanceRequestDto request)
        {
            var result = await _balanceService.ChangeBalanceAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage); 
            }

            return Ok(result);
        }
    }
}
