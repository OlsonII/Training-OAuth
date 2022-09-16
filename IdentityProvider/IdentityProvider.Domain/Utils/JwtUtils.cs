using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityProvider.Domain.Utils;

public static class JwtUtils
{
    public static string GetTokenOwner(this JwtSecurityToken token)
    {
        var username = token?.Claims.FirstOrDefault(claim => claim.Type == "username");
        return username?.Value ?? string.Empty;
    }

    public static string GenerateAccessToken(string username)
    {
        var expirationTime = DateTime.Now.AddDays(7);
        var claims = new List<Claim>()
        {
            new("username", username)
        };
        var jwt = new JwtSecurityToken(claims: claims, expires: expirationTime);
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(jwt);
    }
    
    public static string GenerateIdToken(string username)
    {
        var expirationTime = DateTime.Now.AddDays(7);
        var claims = new List<Claim>()
        {
            new("username", username),
            new("location", "Colombia"),
            new("work", "Developer")
        };
        var jwt = new JwtSecurityToken(claims: claims, expires: expirationTime);
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(jwt);
    }
}