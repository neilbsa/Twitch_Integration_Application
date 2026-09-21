using TwitchLive.Application.Abstractions.Chats;
using TwitchLive.Domain.Abstractions.Results;
using TwitchLive.Domain.Channels.Repository;
using TwitchLive.Domain.Followers.Properties;
using TwitchLive.Domain.Followers.Repository;

namespace TwitchLive.Application.Followers.SendRecognitionToFollower;


public record SendRecognitionToFollowerCommand(FromUserId fromUserId,FromLogin userName, ToUserId toUserId);

public sealed class SendRecognitionToFollowerCommandHandler
{
 
    private readonly IChatService _chatService;
    public SendRecognitionToFollowerCommandHandler(IChatService chatService)
    {
     
        _chatService = chatService;
    }

    public async Task<Result> Handle(SendRecognitionToFollowerCommand command, CancellationToken token)
    {

        try
        {
             await _chatService.SendThanksToFollowerChatAsync(command.fromUserId,command.userName, command.toUserId);
             return Result.Success();
        }catch
        {
            return Result.Failure(new Error("Sending Welcome chat error","SendRecognitionToFollowerCommandHandler throw an error"));
        }
         

    }


}

