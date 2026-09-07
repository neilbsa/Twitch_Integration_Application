using JasperFx.Descriptors;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;

namespace TwitchLive.Application;

public static class DependencyInjections
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }

    public static void ConfigureWolverine(WolverineOptions opt)
    {
         opt.Discovery.IncludeAssembly(typeof(DependencyInjections).Assembly);
     

    }
}
