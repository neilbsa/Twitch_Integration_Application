using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Communication.Events;
using Wolverine.Logging;

namespace TwitchLive.Infrastructure.TwitchManagerSettings.HostedServices;

public sealed class TwitchClientHostedService : IHostedService
{
    private readonly SemaphoreSlim _reconnectLock = new(1, 1);
    private readonly TwitchClient _client;
    private readonly ILogger<TwitchClientHostedService> _logger;
   private readonly ResiliencePipeline _reconnectPipeline;

    public TwitchClientHostedService(TwitchClient client, ILogger<TwitchClientHostedService> logger,ResiliencePipelineProvider<string>  reconnectPipeline)
    {
        _client = client;
        _logger = logger;
        RegisterHandlers();
        _reconnectPipeline = reconnectPipeline.GetPipeline("twitch-websocket");
    }

    private void RegisterHandlers()
    {
        _client.OnConnected += ClientConnected;
        _client.OnDisconnected += ClientDisconnected;
    }
    private void ClientDisconnected(object? sender, OnDisconnectedEventArgs e)
    {
       _logger.LogInformation("Client disconnected");

    }

    private void ClientConnected(object? sender, OnConnectedArgs e)
    {
      _logger.LogInformation("Client connected");
    
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _client.Connect();

    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {

       _client.Disconnect();
        _logger.LogInformation("Twitch client stopped");
    }
}