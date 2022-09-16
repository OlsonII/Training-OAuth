using System.IdentityModel.Tokens.Jwt;
using IdentityProvider.Domain.Utils;
using Newtonsoft.Json;

namespace IdentityProvider.Domain.Models;

public class Authentication
{
    public string IdToken { get; set; }
    public List<Access> Accesses { get; set; }

    public Authentication()
    {
    }

    public Authentication(string username)
    {
        IdToken = JwtUtils.GenerateIdToken(username);
        Accesses = new List<Access>();
    }

    public string GetUsername()
    {
        return new JwtSecurityToken(IdToken).GetTokenOwner();
    }

    public string RegisterAccessToken(string username)
    {
        var accessToken = JwtUtils.GenerateAccessToken(username);
        var access = new Access(accessToken);
        Accesses.Add(access);
        return accessToken;
    }
    
    public void RevokeAccessToken(string accessToken)
    {
        Accesses.RemoveAll(access => access.AccessToken == accessToken);
        var access = new Access(accessToken)
        {
            Allowed = false
        };
        Accesses.Add(access);
    }

    public bool IsFirstAuth()
    {
        return !Accesses.Any();
    }

    public bool IsValidAccessToken(string accessToken)
    {
        var access = Accesses.FirstOrDefault(access => access.AccessToken == accessToken);
        return access?.Allowed ?? false;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}