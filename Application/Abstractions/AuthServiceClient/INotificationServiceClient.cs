using Application.Dtos;
using Domain.Entities;
using Domain.Results;

namespace Application.Abstractions.AuthServiceClient;

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