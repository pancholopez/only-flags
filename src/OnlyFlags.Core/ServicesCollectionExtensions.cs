using Microsoft.Extensions.DependencyInjection;
using OnlyFlags.Core.Application;

namespace OnlyFlags.Core;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddOnlyFlags(this IServiceCollection services)
    {
        services.AddTransient<IFeatureFlagProvider, FeatureFlagFacade>();
        services.AddTransient<IFeatureFlagManager, FeatureFlagFacade>();
        
        return services;
    }
}