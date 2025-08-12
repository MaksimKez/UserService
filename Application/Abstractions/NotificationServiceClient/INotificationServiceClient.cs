using Application.Dtos;
using Domain.Results;

namespace Application.Abstractions.NotificationServiceClient;

public interface INotificationServiceClient
{
    Task<Result> NotifyUserAsync(
        UserDto userDto,
        ListingDto matchingListing,
        CancellationToken cancellationToken = default);
    Task<Result<Dictionary<Guid, string>>> NotifyUsersAsync(
        Dictionary<UserDto, ListingDto> userIdToListingDtos,
        CancellationToken cancellationToken = default);
}