using Common.Constants;
using Common.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using PsIntegrations.Interfaces;

namespace PsIntegrations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Property  
        private readonly IJwtService _jwtHandler;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor  
        public AuthController(IJwtService jwtHandler, IMemoryCache cache, IConfiguration configuration)
        {
            _jwtHandler = jwtHandler;
            _memoryCache = cache;
            _configuration = configuration;
        }
        #endregion

        [HttpGet("AuthenticateParagon")]
        public async Task<IActionResult> AuthenticateParagon()
        {
            try
            {
                var token = GetTokenFromCache();
                if (token == null || !_jwtHandler.ValidateToken(token))
                {
                    token = _jwtHandler.CreateToken();
                    SetTokenInCache(token);
                }
                return Ok(new { Token = token.Token, ExpiresAt = token.ExpiresAt });
            }
            catch (Exception ex)
            {
                return Ok("Unable to generate Token. Please contact support.");
            }
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
