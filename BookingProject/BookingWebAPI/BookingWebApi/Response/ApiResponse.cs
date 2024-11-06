using BookingWebApi.Application.Models;

namespace BookingWebApi.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> Errors { get; set; }

        public ApiResponse(Result<T> result)
        {
            Success = result.IsSuccess;
            Data = result.IsSuccess ? result.Value : default;
            ErrorMessage = result.ErrorMessage;
            Errors = result.Errors;
        }
    }
}
