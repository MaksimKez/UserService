using Application.Dtos;
using Application.Results;

namespace Application.Abstractions.AuthServiceClient;

public interface IAuthServiceClient
{
    Task<Result> NotifyUserAsync(
        Guid userId,
        ListingDto matchingListing,
        CancellationToken cancellationToken = default);
    Task<Result<Dictionary<Guid, string>>> NotifyUsersAsync(
        Dictionary<Guid, ListingDto> userIdToListingDtos,
        CancellationToken cancellationToken = default);
}