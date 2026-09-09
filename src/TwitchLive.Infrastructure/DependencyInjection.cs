using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwitchLive.Domain.Abstractions.UnitOfWork;
using TwitchLive.Domain.Channels.Repository;
using TwitchLive.Domain.TwitchManager;
using TwitchLive.Infrastructure.Repository;
using TwitchLive.Infrastructure.TwitchManagerSettings;
namespace TwitchLive.Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationBuilder builder)
    {
    
        services.AddMemoryCache();
        services.AddDbContext<ApplicationDbContext>(cfg=> cfg.UseSqlite("Data Source=./database/app.db"));
        services.AddScoped<IUnitOfWork>(s=>s.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IChannelRepository,ChannelRepository>();
  
        services.AddScoped<ITwitchManager,TwitchManager>();
              services.AddTwitchLibraryConfiguration(builder);
        return services;
    }



}
