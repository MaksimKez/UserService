using Application.Abstractions.AuthServiceClient;
using Infrastructure.AuthServiceClient;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<INotificationServiceClient, NotificationServiceClient>();
        
        return services;
    }
}
