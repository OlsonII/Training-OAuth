using Microsoft.IdentityModel.Tokens;

namespace IdentityProvider.Domain.Utils;

public static class Base64Utils
{
    public static string GetUsernameFromBase64Token(string token)
    {
        var base64EncodedBytes = Convert.FromBase64String(token);
        var credentials = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        if (credentials.IsNullOrEmpty())
            return string.Empty;
        
        var username = credentials.Split(':')[0];
        return username;
    }
}