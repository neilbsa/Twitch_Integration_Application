
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TwitchLib.Api;
using TwitchLib.Api.Core;
using TwitchLib.Api.Helix.Models.Users.GetUserFollows;
using TwitchLib.Api.Helix.Models.Users.GetUsers;
using TwitchLib.Api.Services;
using TwitchLib.Api.Services.Events.LiveStreamMonitor;
using TwitchLive.Infrastructure.Cache;
using TwitchLive.Infrastructure.TwitchOption.TwitchApiImplementations.HttpHandlers;
using TwitchLive.Infrastructure.TwitchOption.TwitchApiImplementations.RateLimit;

namespace TwitchLive.Infrastructure.TwitchOption.TwitchLibImplementation;

public interface ITwitchLive
{
    void AddToMonitoring(string channelName);
    Task<List<TwitchLib.Api.Helix.Models.Streams.GetFollowedStreams.Stream>> GetFollowedStreams(string userId);
    List<string> GetJoinedChannels();
    Task GetStreams();
    Task<List<Follow>> GetUserFollows(string userId);
    Task<List<User>> GetUsersDetailsAsync(List<string> userNames);
}

public sealed class TwitchLive : ITwitchLive
{

    private readonly ITwitchLiveDataCache _twitchLiveDataCache;
    private readonly IOptions<TwitchOptions> _options;
    private readonly TwitchAPI _twitchApi;
    private readonly LiveStreamMonitorService _liveMonitoringService;
    private readonly TwitchRateLimit _twitchRateLimit;
    private readonly TwitchHttpHandler _twitchHttpHandler;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<TwitchLive> _logger;
    public TwitchLive(
        IOptions<TwitchOptions> options,
        ITwitchLiveDataCache twitchLiveDataCache,
        TwitchRateLimit twitchRateLimit,
        TwitchHttpHandler twitchHttpHandler,
        ILoggerFactory loggerFactory,
        ILogger<TwitchLive> logger)
    {
        _twitchLiveDataCache = twitchLiveDataCache;
        _twitchRateLimit = twitchRateLimit;
        _twitchHttpHandler = twitchHttpHandler;
        _loggerFactory = loggerFactory;
        _options = options;
        _logger = logger;

        var api = new TwitchAPI(_loggerFactory, _twitchRateLimit, new ApiSettings()
        {
            AccessToken = _options.Value.AccessToken,
            ClientId = _options.Value.ClientId
        }, _twitchHttpHandler);

        _twitchApi = api;
        _liveMonitoringService = new LiveStreamMonitorService(_twitchApi);
        _liveMonitoringService.SetChannelsByName(_options.Value.PriorityChannels);
        _liveMonitoringService.OnStreamOnline += StreamOnlines;
        _liveMonitoringService.OnStreamOffline += StreamOfflines;
        _liveMonitoringService.OnStreamUpdate += StreamUpdates;
        _liveMonitoringService.Start();



    }
    private void StreamUpdates(object? sender, OnStreamUpdateArgs e)
    {


        var str = e.Stream.UserLogin;
        var strGameId = e.Stream.GameId;
        var strLang = e.Stream.Language;

        _logger.LogInformation($"updates:{str} {strGameId} {strLang}");
    }
    private void StreamOfflines(object? sender, OnStreamOfflineArgs e)
    {

        var str = e.Stream.UserLogin;
        var strGameId = e.Stream.GameId;
        var strLang = e.Stream.Language;
        _logger.LogInformation($"OFFLINE:{str} {strGameId} {strLang}");
    }
    private void StreamOnlines(object? sender, OnStreamOnlineArgs e)
    {


        var str = e.Stream.UserLogin;
        var strGameId = e.Stream.GameId;
        var strLang = e.Stream.Language;

        Console.WriteLine($"online:{str} {strGameId} {strLang}");
    }


    public List<string> GetJoinedChannels()
    {
        var joinedChannels = _liveMonitoringService.ChannelsToMonitor;
        _logger.LogInformation($"Joined channels: {string.Join(", ", joinedChannels)}");
        return joinedChannels;
    }
    public async Task<List<User>> GetUsersDetailsAsync(List<string> userNames)
    {
        var userDetails = await _twitchApi.Helix.Users.GetUsersAsync(logins: userNames);

        return userDetails.Users.ToList();
    }
    public void AddToMonitoring(string channelName)
    {
        _logger.LogInformation($"Adding {channelName} to monitoring.");
        _liveMonitoringService.ChannelsToMonitor.Add(channelName);
    }
    public async Task GetStreams()
    {

        var tasks = new List<Task>();

        // for (int i = 0; i < 3000; i++)
        // {
        //     tasks.Add(Task.Run(async () =>
        //     {
        //         var streams = await _api.Helix.Streams.GetStreamsAsync(first: 1);
        //         Console.WriteLine($"stream id available: {streams.Streams.Count().ToString()}");
        //     }));
        // }

        // await Task.WhenAll(tasks);

        // var strea    mss = _twitchApi.Helix.Streams.GetStreamsAsync(
        //     first: 100,
        //     languages: new List<string> { "en" },
        //     gameIds: new List<string> { "21779" }
        //     ).GetAwaiter().GetResult();
        // var stringStreamsIds = String.Join(",", streamss.Streams.Select(z=>z.UserId.ToString()));
        // var stringStreamsName = String.Join(",", streamss.Streams.Select(z=>z.UserLogin.ToString()));


        // Console.WriteLine($"stream name available: {stringStreamsName}");
    }
    public async Task<List<TwitchLib.Api.Helix.Models.Streams.GetFollowedStreams.Stream>> GetFollowedStreams(string userId)
    {
        var streams = await _twitchApi.Helix.Streams.GetFollowedStreamsAsync(userId, first: 100);
        if (streams.Data == null || streams.Data.Count() == 0)
        {
            _logger.LogInformation($"No followed streams found for userId: {userId}");
            return new List<TwitchLib.Api.Helix.Models.Streams.GetFollowedStreams.Stream>();
        }
        _logger.LogInformation("Followed streams retrieved successfully. userId: {userId}", userId);
        return streams.Data.ToList();
    }
    public async Task<List<Follow>> GetUserFollows(string userId)
    {
        try
        {
            List<Follow> allFollows = new List<Follow>();
            string? cursor = null;
            int pageCount = 0;

            do
            {
                // Fetch current page
                GetUsersFollowsResponse follows = await _twitchApi.Helix.Users.GetUsersFollowsAsync(
                    fromId: userId,
                    first: 100,
                    after: cursor
                );

                if (follows?.Follows == null || follows.Follows.Length == 0)
                {
                    _logger.LogInformation("No more follows to fetch.");
                    break;
                }

                allFollows.AddRange(follows.Follows);
                pageCount++;

                _logger.LogInformation(
                    $"Page {pageCount}: Fetched {follows.Follows.Length} follows. Total so far: {allFollows.Count}"
                );


                cursor = follows.Pagination?.Cursor;

                if (string.IsNullOrEmpty(cursor))
                {
                    _logger.LogInformation("Pagination complete. No more pages available.");
                    break;
                }

            } while (!string.IsNullOrEmpty(cursor));


            _logger.LogInformation($"Total follows retrieved: {allFollows.Count}");




            return allFollows; // Return the complete list
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching follows for user {userId}");
            throw;
        }
    }






}