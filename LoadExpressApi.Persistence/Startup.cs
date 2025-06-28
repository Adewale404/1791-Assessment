using LoadExpressApi.Persistence.Common;
using LoadExpressApi.Persistence.Context;
//using LoadExpressApi.Persistence.Context.BasicSeeding;
using LoadExpressApi.Persistence.Logging.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoadExpressApi.Persistence;
public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services
            .AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString))
            .AutoAddServices()
            .AddExceptionMiddleware()
            .AddRequestLogging(config);
        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config)
    {
        return builder
        .UseHttpsRedirection()
        // .UseStatusCodePagesWithReExecute("/Error/Error?errorId={0}")
        .UseStaticFiles()
        .UseExceptionMiddleware()
        .UseRequestLogging(config)
        .UseRouting()
        .UseAuthentication()
        .UseAuthorization();
    }
}