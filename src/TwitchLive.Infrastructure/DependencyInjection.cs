using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwitchLive.Infrastructure.TwitchManagerSettings;
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
