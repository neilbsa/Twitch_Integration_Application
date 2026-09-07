using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

using TwitchLive.Infrastructure.Cache;

namespace TwitchLive.Infrastructure.TwitchOption;

public interface ITwitchBot
{
    void ClientStart();
    void JoinChannel(string Channel);
    void LeaveChannel(string ch);
    void SendMessage(string ch, string Message);
    Task StopAsync(CancellationToken cancellationToken);
}

public sealed class TwitchBot : ITwitchBot
{

    private readonly TwitchOptions _options;
    private readonly TwitchClient _client;
    private readonly ITwitchLiveDataCache _cache;
    private readonly ILogger<TwitchBot> _logger;
    public TwitchBot(
        IOptions<TwitchOptions> options, ITwitchLiveDataCache cache, ILogger<TwitchBot> logger)
    {
        _options = options.Value;
        _cache = cache;
        _logger = logger;


        var credentials = new ConnectionCredentials(
            _options.Username,
            _options.OAuthToken);
        _client = new TwitchClient();
        _client.Initialize(
            credentials);
        _client.OnConnected += OnConnected;
        _client.OnMessageReceived += OnMessageReceived;
        _client.OnConnectionError += OnConnectionError;
        ClientStart();

    }
    private void OnConnected(
        object? sender,
        OnConnectedArgs e)
    {
        _logger.LogInformation(
            $"Connected to Twitch as {e.BotUsername}");

        _logger.LogInformation(
              $"Joined channel: {_options.Channel}");
    }
    private void OnMessageReceived(
        object? sender,
        OnMessageReceivedArgs e)
    {
        var username = e.ChatMessage.Username;
        var message = e.ChatMessage.Message;

        Console.WriteLine(
            $"[{username}] {message}");

        if (message.Equals(
            "!hello",
            StringComparison.OrdinalIgnoreCase))
        {
            _client.SendMessage(
                e.ChatMessage.Channel,
                $"Hello @{username}! 👋");
        }

        if (message.Equals(
            "!ping",
            StringComparison.OrdinalIgnoreCase))
        {
            _client.SendMessage(
                e.ChatMessage.Channel,
                "Pong! 🏓");
        }
    }
    private void OnConnectionError(
        object? sender,
        OnConnectionErrorArgs e)
    {
        Console.WriteLine(
            $"Twitch connection error: {e.Error.Message}");



    }


    public void ClientStart()
    {
        _logger.LogInformation("Starting Twitch bot...");

        _client.Connect();
    }

    public Task StopAsync(
        CancellationToken cancellationToken)
    {
        Console.WriteLine("Stopping Twitch bot...");

        if (_client.IsConnected)
        {
            _client.Disconnect();
        }

        return Task.CompletedTask;
    }

    public void JoinChannel(string Channel)
    {
        _client.JoinChannel(Channel);
    }
    public void LeaveChannel(string ch)
    {
        var jndChan = _client.GetJoinedChannel(ch);
        _client.LeaveChannel(jndChan);
    }
    public void SendMessage(string ch, string Message)
    {
        var joinedChannel = _client.GetJoinedChannel(ch);
        _client.SendMessage(joinedChannel, Message);
    }



}
