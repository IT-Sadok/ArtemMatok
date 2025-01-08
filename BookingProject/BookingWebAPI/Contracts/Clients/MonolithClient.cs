using Contracts.DTOs;
using Polly;
using Polly.Registry;
using Polly.Retry;
using Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Clients
{
    public interface IMonolithClient
    {
        Task<Result<UserInfo>> GetUserInfoAsync(string userId);
        Task<string> GetRoleByIdAsync(string userId);
    }
    public class MonolithClient : IMonolithClient
    {
        private readonly HttpClient _httpClient;
        private const string GetUserInfoEndpoint = "api/Account/UserInfo/{0}";
        private const string GetRoleEndpoint = "api/Account/UserRole/{0}";
        private readonly ResiliencePipelineProvider<string> _pipelineProvider;
        public MonolithClient(IHttpClientFactory httpClientFactory, ResiliencePipelineProvider<string> pipelineProvider)
        {
            _httpClient = httpClientFactory.CreateClient("MonolithClient");
            _pipelineProvider = pipelineProvider;
        }

        public async Task<Result<UserInfo>> GetUserInfoAsync(string userId)
        {
            try
            {
                var url = string.Format(GetUserInfoEndpoint, userId);

                var pipeline = _pipelineProvider.GetPipeline("default");
                var userInfo = await pipeline.ExecuteAsync(async x => await _httpClient.GetFromJsonAsync<UserInfo>(url));
                if (userInfo is null) return Result<UserInfo>.Failure("User wasn`t found");
                return Result<UserInfo>.Success(userInfo);
            }
            catch (Exception ex)
            {
                return Result<UserInfo>.Failure(ex.Message);
            }
        }

        public async Task<string> GetRoleByIdAsync(string userId)
        {
            try
            {
                var url = string.Format(GetRoleEndpoint, userId);

                var response = await _httpClient.GetAsync(url);

                var role = await response.Content.ReadAsStringAsync();

                if (role is null) return "Error with getting role";
                return role;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
