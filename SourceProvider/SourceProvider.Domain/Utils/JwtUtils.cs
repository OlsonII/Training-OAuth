using System.IdentityModel.Tokens.Jwt;
using SourceProvider.Domain.Model;

namespace SourceProvider.Domain.Utils;

public static class JwtUtils
{
    public static User? GetTokenOwner(this JwtSecurityToken token)
    {
        var username = token?.Claims.FirstOrDefault(claim => claim.Type == "username");
        var location = token?.Claims.FirstOrDefault(claim => claim.Type == "location");
        var work = token?.Claims.FirstOrDefault(claim => claim.Type == "work");

        if (username == null || location == null || work == null)
            return null;
        
        return new User
        {
            Username = username.Value,
            Location = location.Value,
            Work = work.Value
        };
    }
}