using Application.Abstractions.NotificationServiceClient;
using Application.Dtos;
using Domain.Results;
using Infrastructure.AuthServiceClient.Contracts;
using Infrastructure.AuthServiceClient.Contracts.Interfaces;
using Polly;
using Refit;

namespace Infrastructure.AuthServiceClient;

public class NotificationServiceClient(
    HttpClient httpClient,
    ResiliencePipeline resiliencePipeline,
    INotificationServiceApi notificationServiceApi)
    : BaseHttpService(httpClient, resiliencePipeline), INotificationServiceClient
{
    public async Task<Result> NotifyUserAsync(
        UserDto dto,
        ListingDto matchingListing,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentNullException.ThrowIfNull(matchingListing);

        var pair = new UserListingPairDto { User = dto, Listing = matchingListing };

        try
        {
            var responseMessage = await resiliencePipeline.ExecuteAsync(
                async _ => await notificationServiceApi.NotifySingle(pair),
                ct);
            
            if (responseMessage.IsSuccessStatusCode)
            {
                return Result.Success();
            }

            Console.WriteLine($"Error in NotifyUsersAsync: {(int)responseMessage.StatusCode}");
            Console.WriteLine($"Error in NotifyUsersAsync: {responseMessage.ReasonPhrase}");
            return Result.Failure(responseMessage.ReasonPhrase ?? "Unknown Error");

            
        }
        catch (ApiException apiEx)
        {
            var error = FormatApiError(apiEx);
            return Result.Failure(error);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error in NotifyUserAsync: {ex}");
            throw;
        }
    }

    public async Task<Result<Dictionary<Guid, string>>> NotifyUsersAsync(
        Dictionary<UserDto, ListingDto> usersToListingDtos,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(usersToListingDtos);

        var pairs = usersToListingDtos
            .Select(kvp => new UserListingPairDto { User = kvp.Key, Listing = kvp.Value })
            .ToArray();

        try
        {
            var responseMessage = await resiliencePipeline.ExecuteAsync(
                async _ => await notificationServiceApi.NotifyMultiple(pairs),
                cancellationToken);

            if (responseMessage.IsSuccessStatusCode)
            {
                return Result<Dictionary<Guid, string>>.Success(new Dictionary<Guid, string>());
            }

            Console.WriteLine($"Error in NotifyUsersAsync: {(int)responseMessage.StatusCode}");
            Console.WriteLine($"Error in NotifyUsersAsync: {responseMessage.ReasonPhrase}");
            return Result<Dictionary<Guid, string>>.Failure(responseMessage.ReasonPhrase ?? "Unknown Error");
        }
        catch (ApiException apiEx)
        {
            var error = FormatApiError(apiEx);
            var dict = usersToListingDtos.ToDictionary(kvp => kvp.Key.Id, _ => error);
            return Result<Dictionary<Guid, string>>.PartialFailure([error], dict);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error in NotifyUsersAsync: {ex}");
            throw;
        }
    }

    private static Result ToResult(NotificationResult notificationResult)
        => notificationResult.IsSuccess
            ? Result.Success()
            : Result.Failure(
                string.IsNullOrWhiteSpace(notificationResult.Error) 
                ? "Unknown error"
                : notificationResult.Error
            );

    private static string FormatApiError(ApiException apiEx)
    {
        var errorBody = apiEx.Content ?? string.Empty;
        return string.IsNullOrWhiteSpace(errorBody)
            ? $"HTTP {(int)apiEx.StatusCode} {apiEx.ReasonPhrase}"
            : $"HTTP {(int)apiEx.StatusCode} {apiEx.ReasonPhrase}: {errorBody}";
    }

    private void Print(UserListingPairDto pair)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nName: {pair.User.Name}");
        Console.WriteLine($"Email: {pair.User.Email}\n");
        Console.ResetColor();
    }
}
