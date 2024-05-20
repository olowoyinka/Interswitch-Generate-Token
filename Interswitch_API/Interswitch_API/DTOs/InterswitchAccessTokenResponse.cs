namespace Interswitch_API.DTOs;

public class InterswitchAccessTokenResponse
{
    public string access_token { get; set; } = string.Empty;
    public string token_type { get; set; } = string.Empty;
    public int expires_in { get; set; }
    public string scope { get; set; } = string.Empty;
    public string merchant_code { get; set; } = string.Empty;
    public string requestor_id { get; set; } = string.Empty;
    public string terminalId { get; set; } = string.Empty;
    public string client_name { get; set; } = string.Empty;
    public string client_logo { get; set; } = string.Empty;
    public string payable_id { get; set; } = string.Empty;
    public string client_description { get; set; } = string.Empty;
    public string payment_code { get; set; } = string.Empty;
    public string client_code { get; set; } = string.Empty;
    public string jti { get; set; } = string.Empty;

    public List<string> api_resources { get; set; }
    public Metadata metadata { get; set; }
}

public class Metadata
{
    public string institutionCode { get; set; } = string.Empty;
}