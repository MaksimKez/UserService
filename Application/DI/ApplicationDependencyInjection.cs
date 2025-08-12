using Application.Abstractions;
using Application.Abstractions.UserFilterNotificationService;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DI;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserFilterService, UserFilterService>();
        services.AddScoped<INotificationFinalizer, NotificationFinalizer>();
        services.AddScoped<IUserFilterNotificationService, UserFilterNotificationService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        return services;
    }
}