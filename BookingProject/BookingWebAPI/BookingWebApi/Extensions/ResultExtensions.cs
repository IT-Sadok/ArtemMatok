using BookingWebApi.Application.Models;
using BookingWebApi.Response;
using Microsoft.AspNetCore.Mvc;

namespace BookingWebApi.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToResponse<T>(this Result<T> result)
        {
            var response = new ApiResponse<T>(result);

            if(result.IsSuccess)
            {
                return new OkObjectResult(response.Data);
            }
            else
            {
                return new BadRequestObjectResult(response);
            }
        }
    }
}
