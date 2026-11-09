using fitness.Application.Interfaces;
using fitness.Infrastructure.Settings;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace fitness.Infrastructure.Services
{
    public class TokenService : ITokenService
    {

        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateAccessToken(string userId, string email)
        {
            var claims = new List<Claim>
         {
             new Claim(ClaimTypes.NameIdentifier,userId),
              new Claim(ClaimTypes.Email, email)
         };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var credentials = new SigningCredentials(
              key,
              SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                        issuer: _jwtSettings.Issuer,
                        audience: _jwtSettings.Audience,
                        claims: claims,
                         expires: DateTime.UtcNow.AddMinutes(
                       _jwtSettings.ExpirationInMinutes),
                      signingCredentials: credentials
               );


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            return WebEncoders.Base64UrlEncode(randomBytes);
        }

        public string HashRefreshToken(string refreshToken)
        {
            var hashBytes = SHA256.HashData(
                Encoding.UTF8.GetBytes(refreshToken)
            );

            return Convert.ToBase64String(hashBytes);
        }
    }
}
