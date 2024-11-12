using System.Security.Claims;

namespace BookingWebApi.Helpers
{
    public static class UserHelpers
    {
        public static string GetUserId(HttpContext httpContext)
        {
            return httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
    }
}
