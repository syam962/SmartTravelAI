using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class JwtCookieMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _jwtSecret;

    public JwtCookieMiddleware(RequestDelegate next, string jwtSecret)
    {
        _next = next;
        _jwtSecret = jwtSecret;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var cookieName = "AuthToken";
        if (context.Request.Cookies.TryGetValue(cookieName, out var token))
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = System.Text.Encoding.UTF8.GetBytes(_jwtSecret);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier) ?? principal.FindFirst(JwtRegisteredClaimNames.Sub);

                if (userIdClaim != null)
                {
                    context.Items["UserId"] = userIdClaim.Value;
                }
            }
            catch (SecurityTokenException)
            {
                // Token validation failed, proceed without setting UserId
            }
        }

        await _next(context);
    }
}
