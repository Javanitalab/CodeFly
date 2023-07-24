using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using CodeFly.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;


public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = await context.GetTokenAsync("Bearer", "access_token");

        if (!string.IsNullOrEmpty(token))
        {
            var principal = ValidateToken(token);
            if (principal != null)
            {
                context.User = principal;
            }
        }

        await _next(context);
    }

    private ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,             // Validate the token issuer
            ValidateAudience = true,           // Validate the token audience
            ValidateLifetime = true,           // Validate token expiration
            ValidateIssuerSigningKey = true,   // Validate the token signing key
            ValidIssuer = "your_issuer",       // Replace with your token issuer
            ValidAudience = "your_audience",   // Replace with your token audience
            IssuerSigningKey = new SymmetricSecurityKey(AuthManager.SecretBytes) // Replace with your SecretBytes
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            return principal;
        }
        catch
        {
            // Token validation failed
            return null;
        }
    }
}