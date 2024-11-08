using BookingWebApi.Application.Models;
using BookingWebApi.Application.Response;
using Microsoft.AspNetCore.Mvc;

namespace BookingWebApi.Application.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToResponse<T>(this Result<T> result)
        {
            var response = new ApiResponse<T>(result);

            if (result.IsSuccess)
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
