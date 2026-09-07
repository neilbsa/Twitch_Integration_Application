using TwitchLive.Domain.Abstractions.Results;
using TwitchLive.Domain.Channels;
using TwitchLive.Domain.Channels.Repository;

namespace TwitchLive.Application.Channels.GetListJoinedChannel;

public sealed class GetListMonitoredChannelQueryHandler
{
    private readonly IChannelRepository _repository;
    public GetListMonitoredChannelQueryHandler(IChannelRepository repository)
    {
        _repository = repository;
    }
    public async Task<Result<List<Channel>>> Handle(
        GetListMonitoredChannelQuery query,
        CancellationToken token)
    {
        var list = await 
            _repository.GetAllMonitoredChannelAsync();
        return Result.Success<List<Channel>>(list);
    }
}






