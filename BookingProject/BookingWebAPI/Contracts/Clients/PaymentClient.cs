using Contracts.DTOs.Payment;
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
    public interface IPaymentClient
    {
        Task<Result<bool>> WithdrawBalance(BalanceRequestDto balanceDto);
        Task<Result<bool>> CompensateBalance(BalanceRequestDto balanceDto);

    }
    public class PaymentClient : IPaymentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ResiliencePipelineProvider<string> _pipelineProvider;

        public PaymentClient(IHttpClientFactory httpClientFactory, ResiliencePipelineProvider<string> pipelineProvider)
        {
            _httpClient = httpClientFactory.CreateClient("PaymentClient");
            _pipelineProvider = pipelineProvider;
        }

        public async Task<Result<bool>> WithdrawBalance(BalanceRequestDto balanceDto)
        {
            const string ReserveBalanceEndpoint = "api/Payment/WithdrawBalance";

            try
            {
                var pipeline = _pipelineProvider.GetPipeline("default");
                var result = await pipeline.ExecuteAsync(async x => await _httpClient.PatchAsJsonAsync(ReserveBalanceEndpoint, balanceDto));

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

        public async Task<Result<bool>> CompensateBalance(BalanceRequestDto balanceDto)
        {
            const string CompensateBalanceEndpoint = "api/Payment/CompensateBalance";

            try
            {
                var pipeline = _pipelineProvider.GetPipeline("default");
                var result = await pipeline.ExecuteAsync(async x => await _httpClient.PatchAsJsonAsync(CompensateBalanceEndpoint, balanceDto));

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
