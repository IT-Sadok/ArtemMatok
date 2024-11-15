using BookingWebApi.Application.Common.Models;
using BookingWebApi.Application.Common.Response;
using Microsoft.AspNetCore.Mvc;

namespace BookingWebApi.Application.Common.Extensions
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
