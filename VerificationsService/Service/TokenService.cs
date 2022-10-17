using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VerificationsService.Abstraction;
using VerificationsService.Models.Common;

namespace VerificationsService.Service
{
    public class TokenService : ITokenService
    {
        private readonly AppSettings _appSettings;
        public TokenService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public string GenerateVerificationTokenForNumber(string phoneNumber)
        {
            byte[] key = Encoding.UTF8.GetBytes(_appSettings.Secret);

            var claimsData = new Claim[]
            {
                new Claim("sub", phoneNumber)
            };

            var tokenString = new JwtSecurityToken(
                issuer: _appSettings.TokenIssuer,
                audience: _appSettings.ValidAudience,
                expires: DateTime.UtcNow.AddHours(1),
                claims: claimsData,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenString);
        }
    }
}
