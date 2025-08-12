using Application.Abstractions.AuthServiceClient;
using Application.Dtos;
using Domain.Results;

namespace Infrastructure.AuthServiceClient;

public class NotificationServiceClient : INotificationServiceClient
{

    public async Task<Result> NotifyUserAsync(UserDto dto, ListingDto matchingListing, CancellationToken cancellationToken = default)
    {
        await Task.Delay(1000, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<Dictionary<Guid, string>>> NotifyUsersAsync(Dictionary<UserDto, ListingDto> usersToListingDtos, CancellationToken cancellationToken = default)
    {
        await Task.Delay(1000, cancellationToken);
        return Result<Dictionary<Guid, string>>.Success(new Dictionary<Guid, string>());
    }
}