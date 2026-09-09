using JasperFx.Events.Documents;
using MassTransit.NewIdFormatters;
using TwitchLive.Domain.Channels;
using TwitchLive.Domain.Channels.Properties;
using TwitchLive.Domain.Channels.Repository;

namespace TwitchLive.Infrastructure.Repository;


public sealed class ChannelRepository : IChannelRepository
{



    private readonly ApplicationDbContext _context;
    public ChannelRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public void Add(Channel ch)
    {
        _context.Set<Channel>().Add(ch);
    }

    public async Task<bool> ChannelExistsAsync(UserTwitchLogin channelLogin)
    {
       return await _context.Set<Channel>().AnyAsync(z=>z.Login == channelLogin);
    }

    public async Task<IReadOnlyList<Channel>> GetAllMonitoredChannelAsync()
    {
              return await _context.Set<Channel>().ToListAsync();
    }

    public async Task<Channel?> GetChannelById(Guid id)
    {
            return await _context.Set<Channel>().FindAsync(id);
    }

    public async Task<Channel?> GetChannelByTwitchId(UserTwitchId id)
    {
        return await _context.Set<Channel>().Where(z=>z.UserTwitchId == id).FirstOrDefaultAsync();
    }

    public async Task<Channel?> GetChannelByTwitchLogin(UserTwitchLogin login)
    {
          return await _context.Set<Channel>().Where(z=>z.Login == login).FirstOrDefaultAsync();
    }
}