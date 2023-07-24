using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using DataAccess.Models;

namespace CodeFly.Helper
{
    public class AuthManager
    {
        private const string SecretKey = "fD+402BtCqxSGwkuGc2sncSLKC5m1QcS7UlafnAKllleorGT5YwXVKcq0KbcjxRy"; // Replace with your own secret key
        protected internal static readonly byte[] SecretBytes = Encoding.UTF8.GetBytes(SecretKey);

        public static string GenerateAuthToken(User user)
        {
            var claims = new[]
            {
                new Claim("userid", user.Id.ToString()),
                new Claim("username", user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "https://epita.net",
                Audience = "https://codefly.com",
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(SecretBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static bool ValidateAuthToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(authToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(SecretBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}