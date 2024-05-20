using Interswitch_API.DTOs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Interswitch_API.Handler;

public class AccessTokenService
{
    private readonly HttpClient _httpClient;
    private readonly AppSettings _appSettings;
    
    public AccessTokenService(IHttpClientFactory httpClient, 
                                       IOptions<AppSettings> appSettings)
    {
        _httpClient = httpClient.CreateClient();
        _appSettings = appSettings.Value;
    }

    public async Task<string> GetInterswitchAccessToken()
    {
        var tokenRequestBody = new List<KeyValuePair<string, string>>();
        tokenRequestBody.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
        tokenRequestBody.Add(new KeyValuePair<string, string>("scope", "profile"));

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_appSettings.AccessTokenUrl),
            Content = new FormUrlEncodedContent(tokenRequestBody)
        };

        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

        var byteArray = Encoding.ASCII.GetBytes(_appSettings.ClientID + ":" + _appSettings.SecretKey);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));


        var tokenResponse = await _httpClient.SendAsync(request);

        if (!tokenResponse.IsSuccessStatusCode)
            throw new HttpRequestException($"Error while fetching token. {tokenResponse.ReasonPhrase}");

        await using var stream = await tokenResponse.Content.ReadAsStreamAsync();
        using var streamReader = new StreamReader(stream);
        using var jsonReader = new JsonTextReader(streamReader);

        var result = new JsonSerializer().Deserialize<InterswitchAccessTokenResponse>(jsonReader);

        return result?.access_token ?? "";
    }
}