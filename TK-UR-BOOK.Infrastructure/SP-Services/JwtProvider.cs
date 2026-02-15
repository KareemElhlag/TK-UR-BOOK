using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Infrastructure.SP_Services
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _configuration;
        public JwtProvider( IConfiguration configuration)
        {
          _configuration = configuration;
        }
        public string GenerateToken(User user)
        {
            // Generate claims based on the user information
            var clams = new[]
           {
               new Claim("id", user.Id.ToString()),
               new Claim("username", user.Username),
               new Claim("email", user.Email)
           };
            if (user.Groups != null)
            {
                foreach (var group in user.Groups)
                {
                    clams = clams.Append(new Claim("groups", group.Name)).ToArray();
                }
            }
            // Get the secret key from configuration and create signing credentials
            var secretKey = _configuration["Jwt:Key"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var expireDays = int.Parse(_configuration["Jwt:ExpireDays"] ?? "7");
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: clams,
                expires:DateTime.UtcNow.AddHours(expireDays),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            return principal;
        }
    }

}
