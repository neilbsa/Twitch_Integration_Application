using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using TwitchLib.Api;
using TwitchLib.EventSub.Core.EventArgs.Channel;
using TwitchLib.EventSub.Core.EventArgs.Stream;
using TwitchLib.EventSub.Websockets;
using TwitchLib.EventSub.Websockets.Core.EventArgs;

namespace TwitchLive.Infrastructure.TwitchManagerSettings.HostedServices;

public sealed class TwitchApiHostedService : IHostedService
{
    private readonly TwitchAPI _api;
    private readonly ILogger<TwitchApiHostedService> _logger;
    private readonly EventSubWebsocketClient _subs;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ResiliencePipeline _reconnectPipeline;
    private readonly IHostApplicationLifetime _applicationLifetime;

    private readonly SemaphoreSlim _reconnectLock = new(1, 1);

    public TwitchApiHostedService(
        TwitchAPI api,
        ILogger<TwitchApiHostedService> logger,
        EventSubWebsocketClient subs,
        IServiceScopeFactory scopeFactory,
        ResiliencePipelineProvider<string> reconnectPipeline,
        IHostApplicationLifetime applicationLifetime)
    {
        _api = api;
        _logger = logger;
        _subs = subs;
        _scopeFactory = scopeFactory;
        _applicationLifetime = applicationLifetime;

        _reconnectPipeline =
            reconnectPipeline.GetPipeline("twitch-websocket");
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _subs.WebsocketConnected += WebSocketConnected;
        _subs.WebsocketDisconnected += WebSocketDisconnected;

        RegisterHandlers();

        _logger.LogInformation("Connecting to Twitch EventSub...");

        var isConnected = await _subs.ConnectAsync();

        if (isConnected)
        {
            _logger.LogInformation(
                "Twitch EventSub WebSocket connected. SessionId={SessionId}",
                _subs.SessionId);
        }
        else
        {
            _logger.LogError(
                "Failed to connect to Twitch EventSub WebSocket.");
        }
    }

    private void RegisterHandlers()
    {
 

        _subs.ChannelChatMessage += ChannelChatMessage;
        _subs.ChannelFollow += ChannelFollow;
        _subs.StreamOnline += StreamOnline;
        _subs.StreamOffline += StreamOffline;
    
    }

    private async Task ChannelChatMessage(
        object? sender,
        ChannelChatMessageArgs e)
    {
        using var scope = _scopeFactory.CreateScope();

        var manager =
            scope.ServiceProvider
                .GetRequiredService<ITwitchSubscriptionManager>();

        await manager.ChannelChatMessages(sender, e);
    }

    private async Task ChannelFollow(
        object? sender,
        ChannelFollowArgs e)
    {
        using var scope = _scopeFactory.CreateScope();

        var manager =
            scope.ServiceProvider
                .GetRequiredService<ITwitchSubscriptionManager>();

        await manager.ChannelFollows(sender, e);
    }

    private async Task StreamOnline(
        object? sender,
        StreamOnlineArgs e)
    {
        using var scope = _scopeFactory.CreateScope();

        var manager =
            scope.ServiceProvider
                .GetRequiredService<ITwitchSubscriptionManager>();

        await manager.ChannelOnline(sender, e);
    }

    private async Task StreamOffline(
        object? sender,
        StreamOfflineArgs e)
    {
        using var scope = _scopeFactory.CreateScope();

        var manager =
            scope.ServiceProvider
                .GetRequiredService<ITwitchSubscriptionManager>();

        await manager.ChannelOffline(sender, e);
    }

    private async Task WebSocketConnected(
        object? sender,
        WebsocketConnectedArgs e)
    {
        _logger.LogInformation(
            "Twitch EventSub WebSocket connected. SessionId={SessionId}, RequestedReconnect={RequestedReconnect}",
            _subs.SessionId,
            e.IsRequestedReconnect);

        try
        {
            using var scope = _scopeFactory.CreateScope();

            var manager =
                scope.ServiceProvider
                    .GetRequiredService<ITwitchSubscriptionManager>();

            _logger.LogInformation(
                "Creating EventSub subscriptions for SessionId={SessionId}",
                _subs.SessionId);

            await manager.SubscribeAsync(
                _subs.SessionId,
                _applicationLifetime.ApplicationStopping);

            _logger.LogInformation(
                "EventSub subscriptions created successfully. SessionId={SessionId}",
                _subs.SessionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "FAILED to create EventSub subscriptions. SessionId={SessionId}",
                _subs.SessionId);

            throw;
        }
    }

    private async Task WebSocketDisconnected(
        object? sender,
        WebsocketDisconnectedArgs e)
    {
        if (!await _reconnectLock.WaitAsync(0))
        {
            _logger.LogDebug(
                "WebSocket reconnect already in progress.");

            return;
        }

        try
        {
            _logger.LogWarning(
                "Twitch WebSocket disconnected. Reason={Reason}",
                e);

            await _reconnectPipeline.ExecuteAsync(
                async token =>
                {
                    _logger.LogInformation(
                        "Attempting to reconnect Twitch WebSocket...");

                    var result = await _subs.ReconnectAsync();

                    _logger.LogInformation(
                        "Twitch WebSocket reconnect completed. Result={Result}, SessionId={SessionId}",
                        result,
                        _subs.SessionId);
                },
                _applicationLifetime.ApplicationStopping);
        }
        catch (OperationCanceledException)
            when (_applicationLifetime.ApplicationStopping.IsCancellationRequested)
        {
            _logger.LogInformation(
                "Twitch WebSocket reconnect cancelled because application is stopping.");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Twitch WebSocket reconnect failed.");
        }
        finally
        {
            _reconnectLock.Release();
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Stopping Twitch EventSub WebSocket...");

        _subs.ChannelChatMessage -= ChannelChatMessage;
        _subs.ChannelFollow -= ChannelFollow;
        _subs.StreamOnline -= StreamOnline;
        _subs.StreamOffline -= StreamOffline;

        _subs.WebsocketConnected -= WebSocketConnected;
        _subs.WebsocketDisconnected -= WebSocketDisconnected;

        var disconnected =
            await _subs.DisconnectAsync();

        _logger.LogInformation(
            "Twitch WebSocket disconnected. Result={Result}",
            disconnected);

        _reconnectLock.Dispose();
    }
}