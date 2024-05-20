using Interswitch_API.DTOs;
using System.Net.Http;

namespace Interswitch_API.Services;

public class InterswitchService : IInterswitchService
{
    public readonly HttpClient _httpClient;

    public InterswitchService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    
    public async Task<GetAllBanks?> GetFundsTransferBanksAsync()
    {
        var url = $"quicktellerservice/api/v5/configuration/fundstransferbanks";

        return await _httpClient.GetFromJsonAsync<GetAllBanks>(url);
    }
}