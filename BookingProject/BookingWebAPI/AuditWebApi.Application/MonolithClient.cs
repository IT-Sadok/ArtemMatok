using Contracts.DTOs;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AuditWebApi.Application
{
    public interface IMonolithClient
    {
        Task<Result<UserInfo>> GetUserInfoAsync(string userId);
    }
    public class MonolithClient : IMonolithClient
    {
        private readonly HttpClient _httpClient;
        private const string GetUserInfoEndpoint = "api/Account/UserInfo/{0}";

        public MonolithClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MonolithClient");
        }

        public async Task<Result<UserInfo>> GetUserInfoAsync(string userId)
        {
            try
            {
                var url = string.Format(GetUserInfoEndpoint, userId);

                var userInfo = await _httpClient.GetFromJsonAsync<UserInfo>(url);
                if (userInfo is null) return Result<UserInfo>.Failure("User wasn`t found");

                return Result<UserInfo>.Success(userInfo);
            }
            catch (Exception ex)
            {
                return Result<UserInfo>.Failure(ex.Message);
            }
        }
    }
}
