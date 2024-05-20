using Interswitch_API;
using Interswitch_API.Handler;
using Interswitch_API.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddScoped<AccessTokenService>();

builder.Services.AddScoped<InterswitchAuthorizationDelegatingHandler>();

builder.Services.AddHttpClient<IInterswitchService, InterswitchService>((serviceProvider, httpClient) =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;
    httpClient.BaseAddress = new Uri(settings.BaseUrl);
})
.AddHttpMessageHandler<InterswitchAuthorizationDelegatingHandler>()
.ConfigurePrimaryHttpMessageHandler(() =>
{
    return new SocketsHttpHandler()
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(15)
    };
})
.SetHandlerLifetime(Timeout.InfiniteTimeSpan);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();