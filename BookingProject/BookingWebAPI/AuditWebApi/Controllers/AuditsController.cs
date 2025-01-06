using AuditWebApi.Application.UserAudit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Response;

namespace AuditWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditsController(
        IAuditService _audtiService    
    ) : ControllerBase
    {
        [HttpGet("UserByTime/{userId}")]
        public async Task<IActionResult> GetUserByTime(string userId, [FromQuery]DateTime timestamp)
        {
            var result = await _audtiService.GetUserByTime(userId, timestamp);

            return result.ToResponse();
        }
    }
}
