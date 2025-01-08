using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.EntityDto;
using Payment.Application.Interfaces;

namespace Payment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserBalanceService _userService) : ControllerBase
    {
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserBalane(string userId)
        {
            var result = await _userService.GetUSerBalanceAsync(userId);

            if (result == null)
            {
                return NotFound("User or balance not found");
            }

            return Ok(result);
        }

        [HttpPost("ChangeBalance")]
        public async Task<IActionResult> ChangeUserBalance([FromBody] ChangeBalanceRequest request)
        {
            var result = await _userService.ChangeUserBalanceAsync(request);

            if (result == false)
                return BadRequest();

            return Ok(result);
        }
    }
}
