using Microsoft.Extensions.Caching.Memory;

using Newtonsoft.Json;

using Common.Constants;
using PsIntegrations.Interfaces;
using Common.Models;

namespace PsIntegrations.Services
{
    public class HttpService : IHttpService
    {
        private readonly IJwtService _jwtHandler;
        private readonly IMemoryCache _memoryCache;
        public HttpService(IJwtService jwtHandler, IMemoryCache cache)
        {
            _jwtHandler = jwtHandler;
            _memoryCache = cache;
        }
        public async Task<HttpResponseMessage> PostAsync(string url, object requestBody)
        {
            var token = GetTokenFromCache();
            if (token == null || !_jwtHandler.ValidateToken(token))
            {
                token = _jwtHandler.CreateToken();
                SetTokenInCache(token);
            }
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);
            var response = await httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json"));
            return response;
        }
        private JwtResponse GetTokenFromCache()
        {
            JwtResponse token = null;
            if (_memoryCache.TryGetValue(ParagonConstants.JwtToken, out JwtResponse cacheValue))
            {
                token = cacheValue;
            }
            return token;
        }
        private void SetTokenInCache(JwtResponse jwtResponse)
        {
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(60),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(60),
            };
            _memoryCache.Set(ParagonConstants.JwtToken, jwtResponse, cacheExpiryOptions);
        }
    }
}
