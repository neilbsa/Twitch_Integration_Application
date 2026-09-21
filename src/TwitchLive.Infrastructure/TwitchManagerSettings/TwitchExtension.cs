using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TwitchLib.Api;
using TwitchLib.Api.Core;
using TwitchLib.Api.Core.Interfaces;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.EventSub.Websockets.Extensions;
using TwitchLive.Application.Abstractions.Chats;
using TwitchLive.Infrastructure.TwitchManagerSettings.HostedServices;
using TwitchLive.Infrastructure.TwitchManagerSettings.TwitchApi.Configurations;
using TwitchLive.Infrastructure.TwitchManagerSettings.TwitchApi.EventSubs;


namespace TwitchLive.Infrastructure.TwitchManagerSettings;

public static class TwitchLibDependencyInjections
{
    public static IServiceCollection AddTwitchLibraryConfiguration(this IServiceCollection services,IConfigurationBuilder config)
    {

            var buildConfig = config.Build();


            services.Configure<TwitchOptions>(
                     buildConfig.GetSection(TwitchOptions.SectionName));

             services.AddOptions<TwitchOptions>()
                    .Validate(options => !string.IsNullOrEmpty(options.ClientId), 
                        "ClientId is required")
                
                    .Validate(options => !string.IsNullOrEmpty(options.Username), 
                        "BotUsername is required")
                    .Validate(options => !string.IsNullOrEmpty(options.BotId), 
                        "BotId is required");


        services.AddSingleton<IRateLimiter,TwitchRateLimit>();             
        services.AddSingleton<IHttpCallHandler,TwitchCustomHttpHandler>();          

    
        services.AddSingleton<TwitchClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<TwitchOptions>>().Value;
            var client = new TwitchClient();
            var connectionCredential = new ConnectionCredentials(options.Username,options.OAuthToken);
            client.Initialize(connectionCredential);    
         
            return client;
        });

        services.AddSingleton<TwitchAPI>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<TwitchOptions>>().Value;
            var limiter = sp.GetRequiredService<IRateLimiter>();
            var httpHandler = sp.GetRequiredService<IHttpCallHandler>();

            var logger = LoggerFactory.Create(cfg =>
            {
                cfg.AddConsole(); 

            });
            var apiSettings = new ApiSettings()
            {
            
                ClientId = options.ClientId,
                AccessToken= options.AccessToken,
           
            };
            var client = new TwitchAPI(logger,limiter,apiSettings,httpHandler);
           return client;
        });
        services.AddSingleton<ITwitchEventSubService,TwitchEventSubService>();
        services.AddSingleton<IChatService,ChatService>();
        services.AddScoped<ITwitchChannelService, TwitchChannelService>();
        services.AddScoped<ITwitchSubscriptionManager, TwitchSubscriptionManager>();
        services.AddTwitchLibEventSubWebsockets();
        services.AddHostedService<TwitchApiHostedService>();
        services.AddHostedService<TwitchClientHostedService>();
        return services;

    }
}