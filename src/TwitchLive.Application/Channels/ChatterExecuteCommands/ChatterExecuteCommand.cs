using System.Runtime.CompilerServices;
using TwitchLive.Domain.Channels.Properties;

namespace TwitchLive.Application.Channels.ChatterExecuteCommands;




public record ChatterExecuteCommand(UserTwitchLogin from, string command);

public sealed class ChatterExecuteCommandHandler
{
    public async Task Handle(ChatterExecuteCommand command , CancellationToken token)
    {
        //get the command
    }

}