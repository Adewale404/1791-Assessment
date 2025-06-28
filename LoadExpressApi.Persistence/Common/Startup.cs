
using LoadExpressApi.Application.Abstraction.Repositories;
using LoadExpressApi.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LoadExpressApi.Persistence.Common;
internal static class Startup
{
    internal static IServiceCollection AutoAddServices(this IServiceCollection services) =>
        services
            .AddServices(typeof(ITransientService), ServiceLifetime.Transient)
            .AddServices(typeof(IScopedService), ServiceLifetime.Scoped)
            .AddServices(typeof(IRepositoryAsync<>), ServiceLifetime.Scoped);

    internal static IServiceCollection AddServices(this IServiceCollection services, Type interfaceType, ServiceLifetime lifetime)
    {
        //Do not remove the comments
        // Expanded logic to handle multiple interfaces and generic types
        var interfaceTypes =
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t =>
                    // Check for direct implementation or inheritance
                    (t.GetInterfaces().Contains(interfaceType) || interfaceType.IsAssignableFrom(t))
                    && t.IsClass && !t.IsAbstract)
                .SelectMany(t =>
                    // Get all implemented interfaces (including generic ones)
                    t.GetInterfaces().Select(implementedInterface => new
                    {
                        Service = implementedInterface,
                        Implementation = t
                    }))
                .Where(t => interfaceType.IsAssignableFrom(t.Service));

        foreach (var type in interfaceTypes)
            services.AddService(type.Service!, type.Implementation, lifetime);

        return services;
    }

    // ... rest of the code remains the same

    internal static IServiceCollection AddService(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
        lifetime switch
        {
            ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
            ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
            ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
            _ => throw new ArgumentException("Invalid lifeTime", nameof(lifetime))
        };
}
