using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TwitchLive.Infrastructure.Cache;
using TwitchLive.Infrastructure.TwitchOption;
using TwitchLive.Infrastructure.TwitchOption.TwitchApiImplementations.HttpHandlers;
using TwitchLive.Infrastructure.TwitchOption.TwitchApiImplementations.RateLimit;


namespace TwitchLive.Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration builder)
    {

        services.AddLogging(config =>
        {
            config.AddConsole();
            config.AddDebug(); // Also log to debug output
            config.SetMinimumLevel(LogLevel.Debug);
        });
        services.Configure<TwitchOptions>(
             builder.GetSection("TwitchOptions"));
        services.AddMemoryCache();
        services.AddSingleton<TwitchHttpHandler>();
        services.AddSingleton<TwitchRateLimit>();
        services.AddSingleton<ITwitchLiveDataCache, TwitchLiveDataCache>();
        services.AddSingleton<ITwitchBot,TwitchBot>();
   
        return services;
    }



}
