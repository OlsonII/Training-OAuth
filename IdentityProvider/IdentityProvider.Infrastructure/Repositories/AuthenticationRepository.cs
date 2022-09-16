using IdentityProvider.Domain.Models;
using Newtonsoft.Json;

namespace IdentityProvider.Infrastructure.Repositories;

public class AuthenticationRepository
{
    public void SaveAuthentication(Authentication authentication)
    {
        using var outputFile = new StreamWriter($"{authentication.GetUsername()}.json");
        outputFile.WriteLine(authentication);
    }

    public Authentication? GetAuthentication(string username)
    {
        using var streamReader = new StreamReader($"{username}.json");
        var json = streamReader.ReadToEnd();
        var authentication = JsonConvert.DeserializeObject<Authentication>(json);
        return authentication;
    }
}