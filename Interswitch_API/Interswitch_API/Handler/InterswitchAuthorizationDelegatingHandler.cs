using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System.Net.Http.Headers;

namespace Interswitch_API.Handler;

public class InterswitchAuthorizationDelegatingHandler : DelegatingHandler
{
    private readonly AppSettings _appSettings;
    private readonly AccessTokenService _accessTokenService;

    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy = Policy<HttpResponseMessage>
                                                                            .Handle<HttpRequestException>()
                                                                            .RetryAsync(2);

    public InterswitchAuthorizationDelegatingHandler(AccessTokenService accessTokenService, IOptions<AppSettings> appSettings)
    {
        _accessTokenService = accessTokenService;
        _appSettings = appSettings.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authToken = await _accessTokenService.GetInterswitchAccessToken();

        request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, authToken);
        request.Headers.Add("Terminalid", _appSettings.Terminalid);
        request.Headers.Add("accept", "application/json");

        var policyResult = await _retryPolicy.ExecuteAndCaptureAsync(() => base.SendAsync(request, cancellationToken));

        if (policyResult.Outcome == OutcomeType.Failure)
        {
            throw new HttpRequestException("Something went wrong", policyResult.FinalException);
        }

        policyResult.Result.EnsureSuccessStatusCode();

        var ff = policyResult.Result;

        return ff;
    }
}