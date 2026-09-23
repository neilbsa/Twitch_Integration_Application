using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwitchLive.Domain.Abstractions.UnitOfWork;
using TwitchLive.Domain.Channels.Repository;
using TwitchLive.Infrastructure.Cache;
using TwitchLive.Infrastructure.Repository;
using TwitchLive.Infrastructure.TwitchManagerSettings;
using Polly;
using Polly.Retry;
using TwitchLive.Infrastructure.BackgroundServices;
using TwitchLive.Domain.Followers.Repository;

namespace TwitchLive.Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationBuilder builder)
    {



        services.AddResiliencePipeline("twitch-websocket", pipeline =>
        {
            
                pipeline.AddRetry(new RetryStrategyOptions
                {
                    MaxRetryAttempts = 5,
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    Delay=TimeSpan.FromSeconds(1)
                });


        });
        services.AddMemoryCache();
        services.AddDbContext<ApplicationDbContext>(cfg=> cfg.UseSqlite("Data Source=./database/app.db"));
        services.AddScoped<IUnitOfWork>(s=>s.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IChannelRepository,ChannelRepository>();
          services.AddScoped<IFollowerRepository,FollowerRepository>();
        services.AddSingleton<ICaching,Caching>();
 
        services.AddTwitchLibraryConfiguration(builder);
        services.AddHostedService<GetAllTheFollowersOnChannel>();
        return services;
    }



}
