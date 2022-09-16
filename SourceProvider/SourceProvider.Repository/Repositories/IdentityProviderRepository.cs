using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using SourceProvider.Domain.Model;

namespace SourceProvider.Repository.Repositories;

public class IdentityProviderRepository
{
    private const string IdentityProviderUrl = "https://localhost:7271/auth";
    
    public async Task<string> Authorize(string accessToken)
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(IdentityProviderUrl);
        var request = new HttpRequestMessage(HttpMethod.Post, httpClient.BaseAddress);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return string.Empty;

        var content = await response.Content.ReadAsStringAsync();
        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(content);
        return authResponse?.IdToken ?? string.Empty;
    }
}