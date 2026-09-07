using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TwitchLive.Infrastructure.Cache;
using TwitchLive.Infrastructure.TwitchManagerSettings;
using TwitchLive.Infrastructure.TwitchOption;
using TwitchLive.Infrastructure.TwitchOption.TwitchApiImplementations.HttpHandlers;
using TwitchLive.Infrastructure.TwitchOption.TwitchApiImplementations.RateLimit;


namespace TwitchLive.Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationBuilder builder)
    {

        services.AddMemoryCache();
        services.AddTwitchLibraryConfiguration(builder);
   
        return services;
    }



}
