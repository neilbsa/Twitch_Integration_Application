using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Logging;
using TwitchLib.Api;
using TwitchLib.Api.Services;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;


namespace TwitchLive.Infrastructure.TwitchManagerSettings.Services;



public sealed class TwitchClientService
{
    private readonly TwitchClient _client;
    private readonly ILogger<TwitchClientService> _logger;
    private readonly TwitchAPI _twitchApi ;

    public TwitchClientService(TwitchClient client, ILogger<TwitchClientService> logger, TwitchAPI twitchApi)
    {

        _client = client;
        _logger = logger;

        RegisterHandlers();
        _twitchApi = twitchApi;
    }

    private void RegisterHandlers()
    {
    _logger.LogInformation("Registering Twitch handlers...");

  
    _client.OnConnected += ClientConnected;
    _client.OnDisconnected += ClientDisconnected;
    _client.OnConnectionError += ClientconnectionError;
    _client.OnReconnected += ClientReconnected;
    _client.OnIncorrectLogin += IncorrectLogin;





    _client.OnMessageReceived += MessageReceived;
    _client.OnMessageSent += MessageSent;

    _client.OnLeftChannel += OnLeftChannel;
    _client.OnJoinedChannel += OnJoinChannel;
  

    _client.OnChatCommandReceived += CommandRecieved;
    _client.OnNewSubscriber += NewSubscriber;


    _client.OnUserJoined += UserJoined;
    _client.OnUserLeft += UserLeft;
 


    _client.OnModeratorJoined += ModeratorJoined;
    _client.OnModeratorLeft += ModeratorLeft;
    _client.OnBanned += OnBanned;
    _client.OnUserBanned += OnUserBaned;



    _logger.LogInformation("Twitch handlers registered.");



    
    }

    private void OnLeftChannel(object? sender, OnLeftChannelArgs e)
    {
         var channel = e.Channel;
          var botUserName = e.BotUsername;
          _logger.LogInformation($"botUserName {botUserName} left the channel { channel}");
    }

    private void OnJoinChannel(object? sender, OnJoinedChannelArgs e)
    {
         var channel = e.Channel;
         var botUserName = e.BotUsername;
          _logger.LogInformation($"botUserName {botUserName} joined the channel { channel}");
    }

    private void ModeratorJoined(object? sender, OnModeratorJoinedArgs e)
    {

        var moderatorName = e.Username;
        var channel = e.Channel;
       _logger.LogInformation($"Moderator {moderatorName} joined the channel { channel}");
    }

    private void ModeratorLeft(object? sender, OnModeratorLeftArgs e)
    {
            var moderatorName = e.Username;
        var channel = e.Channel;
       _logger.LogInformation($"Moderator {moderatorName} left the channel { channel}");
    }

    private void OnBanned(object? sender, OnBannedArgs e)
    {
     var message = e.Message;
        var channel = e.Channel;
       _logger.LogInformation($"channel {channel} banned with message  { message}");
    }

    private void OnUserBaned(object? sender, OnUserBannedArgs e)
    {
         var userBanned = e.UserBan;
        
       _logger.LogInformation($"user {userBanned} was banned");
    }

    private void IncorrectLogin(object? sender, OnIncorrectLoginArgs e)
    {
    _logger.LogError(
        "TWITCH INCORRECT LOGIN: {Username}",
        e.Exception);
    }


    public void Connect()
    {
          _logger.LogInformation(
        "Connect() called. IsConnected = {IsConnected}",
        _client.IsConnected);

            if (!_client.IsConnected)
            {
                _logger.LogInformation(
                    "Calling TwitchClient.Connect()...");

                _client.Connect();

                _logger.LogInformation(
                    "TwitchClient.Connect() returned. IsConnected = {IsConnected}",
                    _client.IsConnected);
            }
    }

    public void Disconnect()
    {
        if (_client.IsConnected)
        {
            _logger.LogInformation("Disconnecting from Twitch...");

            _client.Disconnect();
        }
    }



    private void UserLeft(object? sender, OnUserLeftArgs e)
    {
        var User = e.Username;
        var channel = e.Channel;
        _logger.LogInformation($"user {User} left in channel {channel} !");
    }

    private void UserJoined(object? sender, OnUserJoinedArgs e)
    {
         var User = e.Username;
        var channel = e.Channel;
        _logger.LogInformation($"user {User} joined in channel {channel} !");
    }

    private void NewSubscriber(object? sender, OnNewSubscriberArgs e)
    {
        var User = e.Subscriber.Login;
        var channel = e.Channel;
        _logger.LogInformation($"user {User} subscribed in channel {channel} !");
    }

    private void ClientReconnected(object? sender, OnReconnectedEventArgs e)
    {   _logger.LogWarning(
        "Twitch reconnected. IsConnected={IsConnected}",
        _client.IsConnected);
    }

    private void MessageSent(object? sender, OnMessageSentArgs e)
    {
        var senderDisplayname = e.SentMessage.DisplayName;
        var isSub = e.SentMessage.IsSubscriber;
        var message = e.SentMessage.Message;
        var userType = e.SentMessage.UserType;
        var channel = e.SentMessage.Channel;

        _logger.LogInformation($"user {senderDisplayname} sent a message in channel {channel} that is userTYype {userType} message: {message} and a subscriber {isSub} !");
   
    }

    private void CommandRecieved(object? sender, OnChatCommandReceivedArgs e)
    {
        var command = e.Command.CommandText;
        var userSender = e.Command.ChatMessage.DisplayName;
        var channel = e.Command.ChatMessage.Channel;
           _logger.LogInformation($"user {userSender} send a command to {channel} and command was {command}");
   
    }

    private void MessageReceived(object? sender, OnMessageReceivedArgs e)
    {
           var senderDisplayname = e.ChatMessage.DisplayName;
        var isSub = e.ChatMessage.IsSubscriber;
        var message = e.ChatMessage.Message;
        var userType = e.ChatMessage.UserType;
        var channel = e.ChatMessage.Channel;

        _logger.LogInformation($"user {senderDisplayname} recieved a message in channel {channel} that is userTYype {userType} message: {message} and a subscriber {isSub} !");
   
    }
    private void ClientconnectionError(object? sender, OnConnectionErrorArgs e)
    {
        _logger.LogError(
        e.Error.Message,
        "Twitch client connection error");
    }

    private void ClientDisconnected(object? sender, OnDisconnectedEventArgs e)
    {
              _logger.LogWarning(
        "Twitch disconnected. IsConnected={IsConnected}",
        _client.IsConnected);
    }

    private void ClientConnected(object? sender, OnConnectedArgs e)
    {
         _logger.LogInformation($"client connected!");
   

       
    }
}