using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Channels.GetChannelFollowers;
using TwitchLib.Api.Helix.Models.Channels.SendChatMessage;
using TwitchLib.Api.Helix.Models.Users.GetUserFollows;
using TwitchLive.Application.Followers.CreateNewFollower;
using Wolverine;

namespace TwitchLive.Infrastructure.BackgroundServices;



public sealed class GetAllTheFollowersOnChannel : BackgroundService
{

    private readonly ILogger<GetAllTheFollowersOnChannel> _logger;
   private readonly IServiceScopeFactory _scopeFactory;
    private readonly TwitchAPI _api;
    public GetAllTheFollowersOnChannel(ILogger<GetAllTheFollowersOnChannel> logger, TwitchAPI api, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
      
        _api = api;
        _scopeFactory = scopeFactory;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
      
        var scope = _scopeFactory.CreateScope();
        var _bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
        while (!stoppingToken.IsCancellationRequested)
        {
               _logger.LogInformation("this is coming from a background service");
            string broadCasterId = "1536964012";
           

            string? cursor = string.Empty;

            List<ChannelFollower> followers = new List<ChannelFollower>();
            do
            {

                var broadCasterFollowerData=  await _api.Helix.Channels.GetChannelFollowersAsync(broadcasterId :broadCasterId,first:50,after:cursor);
                var retrievedFollowers =broadCasterFollowerData.Data;


                followers.AddRange(retrievedFollowers);



                if(broadCasterFollowerData?.Pagination?.Cursor == null)
                {
                    cursor = broadCasterFollowerData?.Pagination?.Cursor;

                }
            } while(cursor != null);


            foreach(var followersItem in followers)
            {
                   
                    var dtofollower = new FollowerDTO()
                    {
                         FollowedAt = followersItem.FollowedAt,
                          FromLogin = followersItem.UserLogin,
                           FromUserId = followersItem.UserId,
                            FromUserName = followersItem.UserName,
                             ToLogin = "musicfortheworld__",
                              ToUserId = broadCasterId,
                               ToUserName= "musicfortheworld__"
                    };
                   
                   
                   var command = new CreateNewFollowerCommand(dtofollower);

                   await _bus.PublishAsync(command);

            }

            //     string broadCasterId = "1536964012";
            //     string senderId = "1535275845";
            //     string message = "THIS IS A TEST MESSAGE";


            //     var sendChatMessageRequest = new SendChatMessageRequest()
            //     {
            //           BroadcasterId = broadCasterId,
            //            SenderId = senderId,
            //             Message=message,
                         
            //     };
            //    await _api.Helix.Chat.SendChatMessage(sendChatMessageRequest);

            await Task.Delay(1000000000);
        }
        
    }
    
}