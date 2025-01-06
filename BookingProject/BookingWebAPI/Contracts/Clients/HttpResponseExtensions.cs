using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Contracts.Clients
{
    public class ErrorResponse
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> Errors { get; set; }
    }

    public static class HttpResponseExtensions
    {
        public static async Task<string> ExtractErrorMessage(HttpResponseMessage response)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content, options);

                if (errorResponse != null)
                {
                    if (!string.IsNullOrEmpty(errorResponse.ErrorMessage))
                        return errorResponse.ErrorMessage;

                    if (errorResponse.Errors != null && errorResponse.Errors.Any())
                        return string.Join(", ", errorResponse.Errors);
                }

                return "Unknown error format.";
            }
            catch (JsonException)
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}

