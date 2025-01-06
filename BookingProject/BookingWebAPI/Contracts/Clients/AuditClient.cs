using Contracts.DTOs.Audit;
using Polly.Registry;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Clients
{
    public interface IAuditClient
    {
        Task<Result<bool>> CreateAuditBooking(AuditBookingCreateDto bookingDto);
        Task<Result<bool>> RemoveAuditBooking(RemoveAuditBookingDto bookingDto);
    }
    public static class QueryStringExtensions
    {
        public static string ToQueryParamsString(this RemoveAuditBookingDto bookingDto)
        {
            return $"?UserId={bookingDto.UserId}&ApartamentId={bookingDto.ApartamentId}&StartDate={bookingDto.StartDate}";
        }
    }
    public class AuditClient : IAuditClient
    {
        private readonly HttpClient _httpClient;
        private readonly ResiliencePipelineProvider<string> _pipelineProvider;

        public AuditClient(IHttpClientFactory httpClientFactory, ResiliencePipelineProvider<string> pipelineProvider)
        {
            _httpClient = httpClientFactory.CreateClient("AuditClient");
            _pipelineProvider = pipelineProvider;
        }

        private const string CreateAuditBookingEndpoint = "api/bookingAudits";
        public async Task<Result<bool>> CreateAuditBooking(AuditBookingCreateDto bookingDto)
        {
            try
            {
                var pipeline = _pipelineProvider.GetPipeline("default");
                var result = await pipeline.ExecuteAsync(async x => await _httpClient.PostAsJsonAsync(CreateAuditBookingEndpoint, bookingDto));

                if (!result.IsSuccessStatusCode)
                {
                    var errorMessage = await HttpResponseExtensions.ExtractErrorMessage(result);
                    return Result<bool>.Failure(errorMessage);
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }

        private const string DeleteAuditBookingEndpoint = "api/bookingAudits";
        public async Task<Result<bool>> RemoveAuditBooking(RemoveAuditBookingDto bookingDto)
        {
            try
            {
                var pipeline = _pipelineProvider.GetPipeline("default");

                var result = await pipeline.ExecuteAsync(async x => 
                    await _httpClient.DeleteAsync(DeleteAuditBookingEndpoint + bookingDto.ToQueryParamsString()));

                if (!result.IsSuccessStatusCode)
                {
                    var errorMessage = await HttpResponseExtensions.ExtractErrorMessage(result);
                    return Result<bool>.Failure(errorMessage);
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }
    }
}
