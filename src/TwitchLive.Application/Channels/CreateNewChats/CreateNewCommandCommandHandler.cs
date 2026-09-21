using TwitchLive.Application.Abstractions.Chats;
using TwitchLive.Domain.Abstractions.Results;
using TwitchLive.Domain.Channels.Repository;
using Wolverine;

namespace TwitchLive.Application.Channels.CreateNewChats;

public sealed class CreateNewCommandCommandHandler
{
    
    public readonly IMessageBus _bus;
    public readonly IChannelRepository _repo;
    private readonly IChatService _chatService;
    public CreateNewCommandCommandHandler(IMessageBus bus, IChannelRepository repo, IChatService chatService)
    {
        _bus = bus;
        _repo = repo;
        _chatService = chatService;
    }


    public async Task<Result> Handle(CreateNewChatCommand command, CancellationToken token)
    {
        if(!(command.Message == null || command.Message.Length == 0))
        {
                //get user
            var user = await _repo.GetChannelByTwitchLogin(command.messageTo);

            if(user is not null)
            {
                await _chatService.CreateChatToUser(command.messageTo,command.messageFrom,command.Message);
            }

            return Result.Success();
        }
        else
        {
            return Result.Failure(new Error("Message","Cannot send message to user without message"));
        }
            
    }



}