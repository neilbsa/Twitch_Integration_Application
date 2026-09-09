using Microsoft.Extensions.Hosting;

namespace TwitchLive.Infrastructure.TwitchManagerSettings;

public sealed class TwitchClientHostedService : IHostedService
{
    private readonly TwitchClientService _twitchClientService;

    public TwitchClientHostedService(
        TwitchClientService twitchClientService)
    {
        _twitchClientService = twitchClientService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _twitchClientService.Connect();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _twitchClientService.Disconnect();

        return Task.CompletedTask;
    }
}
