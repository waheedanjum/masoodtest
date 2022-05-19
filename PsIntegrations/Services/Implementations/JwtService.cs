using PsIntegrations.Interfaces;
using Common.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Repository.Interface;
using Microsoft.Extensions.Configuration;

namespace PsIntegrations.Services
{
    public class JwtService : IJwtService
    {
        #region Property  

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor  
        public JwtService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        #endregion

        
        public JwtResponse CreateToken()
        {
            var privateKey = Convert.FromBase64String(_configuration.GetSection("ParagonConfig:RsaPrivateKey").Value.Trim());
            using RSA rsa = RSA.Create();
            rsa.ImportPkcs8PrivateKey(privateKey, out _);

            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
            {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };
            var now = DateTime.Now;
            var expire = DateTime.Now.AddHours(1);
            var unixTimeSecondsNow = new DateTimeOffset(now).ToUnixTimeSeconds();
            var unixTimeSecondsExp = new DateTimeOffset(expire).ToUnixTimeSeconds();

            var jwt = new JwtSecurityToken(
                claims: new Claim[] {
                    new Claim(JwtRegisteredClaimNames.Sub, "0051n000006N9GJAA0", ClaimValueTypes.String),
                     new Claim(JwtRegisteredClaimNames.Iat, unixTimeSecondsNow.ToString(), ClaimValueTypes.Integer64),
                    new Claim(JwtRegisteredClaimNames.Exp, unixTimeSecondsExp.ToString(), ClaimValueTypes.Integer64),
                },
                notBefore: now,
                expires: now.AddMinutes(60),
                signingCredentials: signingCredentials
            );
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            var paragonJwt = new JwtResponse
            {
                Token = token,
                ExpiresAt = unixTimeSecondsExp
            };
            InsertToken(paragonJwt);
            return paragonJwt;
        }
        public bool ValidateToken(JwtResponse token)
        {

            //var publicKey = _settings.RsaPublicKey.ToByteArray();

            //using RSA rsa = RSA.Create();
            //rsa.ImportRSAPublicKey(publicKey, out _);

            //var validationParameters = new TokenValidationParameters
            //{
            //    ValidateIssuer = true,
            //    ValidateAudience = true,
            //    ValidateLifetime = true,
            //    ValidateIssuerSigningKey = true,
            //    //ValidIssuer = _settings.Issuer,
            //    //ValidAudience = _settings.Audience,
            //    IssuerSigningKey = new RsaSecurityKey(rsa),
            //    CryptoProviderFactory = new CryptoProviderFactory()
            //    {
            //        CacheSignatureProviders = false
            //    }
            //};

            try
            {
                //var handler = new JwtSecurityTokenHandler();
                //handler.ValidateToken(token, validationParameters, out var validatedSecurityToken);
                if (token == null || token.ExpiresAt < new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds())
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool InsertToken(JwtResponse token)
        {
            try
            {
                _unitOfWork.ParagonJWTRepository.Insert(MapParagonJWT(token));
                _unitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private Repository.Entities.ParagonJWT MapParagonJWT(JwtResponse token)
        {
            return new Repository.Entities.ParagonJWT
            {
                Token = token.Token,
                ExpireAt = token.ExpiresAt,
                IsDeleted = false,
                CreatedOn = DateTime.Now,
            };
        }
    }

}
