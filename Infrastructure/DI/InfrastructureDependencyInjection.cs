using Application.Abstractions.NotificationServiceClient;
using Infrastructure.AuthServiceClient;
using Infrastructure.AuthServiceClient.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using Refit;


namespace Infrastructure.DI;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddRefitClient<INotificationServiceApi>()
            .ConfigureHttpClient((serviceProvider, httpClient) =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<NotificationClientSettings>>().Value;
                httpClient.BaseAddress = new Uri(settings.BaseUrl);
            });
        
        services.AddHttpClient<NotificationServiceClient>((serviceProvider, client) =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<NotificationClientSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
        });
        
        services.AddSingleton<ResiliencePipeline>(serviceProvider =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<NotificationClientSettings>>().Value;
            
            return new ResiliencePipelineBuilder()
                .AddRetry(new RetryStrategyOptions
                {
                    MaxRetryAttempts = settings.RetryCount,
                    Delay = TimeSpan.FromSeconds(settings.RetryDelaySeconds),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = settings.UseJitter,
                    ShouldHandle = new PredicateBuilder()
                        .Handle<HttpRequestException>()
                        .Handle<TaskCanceledException>()
                        .Handle<TimeoutRejectedException>()
                })
                .AddCircuitBreaker(new CircuitBreakerStrategyOptions
                {
                    FailureRatio = 0.5,
                    SamplingDuration = TimeSpan.FromSeconds(10),
                    MinimumThroughput = settings.CircuitBreakerFailureThreshold,
                    BreakDuration = TimeSpan.FromSeconds(settings.CircuitBreakerBreakDurationSeconds),
                    ShouldHandle = new PredicateBuilder()
                        .Handle<HttpRequestException>()
                        .Handle<TaskCanceledException>()
                        .Handle<TimeoutRejectedException>()
                })
                .AddTimeout(TimeSpan.FromSeconds(settings.TimeoutSeconds))
                .Build();
        });

        
        services.AddScoped<INotificationServiceClient, NotificationServiceClient>();
        
        return services;
    }
}
