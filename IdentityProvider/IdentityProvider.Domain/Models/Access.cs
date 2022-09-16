namespace IdentityProvider.Domain.Models;

public class Access
{
    public string AccessToken { get; set; }
    public bool Allowed { get; set; }

    public Access()
    {
    }

    public Access(string accessToken, bool allowed = true)
    {
        AccessToken = accessToken;
        Allowed = allowed;
    }
}