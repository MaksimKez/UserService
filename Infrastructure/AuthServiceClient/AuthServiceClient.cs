using Application.Abstractions.AuthServiceClient;
using Application.Dtos;
using Application.Results;

namespace Infrastructure.AuthServiceClient;

public class AuthServiceClient : IAuthServiceClient
{

    public async Task<Result> NotifyUserAsync(Guid userId, ListingDto matchingListing, CancellationToken cancellationToken = default)
    {
        await Task.Delay(1000, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<Dictionary<Guid, string>>> NotifyUsersAsync(Dictionary<Guid, ListingDto> userIdToListingDtos, CancellationToken cancellationToken = default)
    {
        await Task.Delay(1000, cancellationToken);
        return Result<Dictionary<Guid, string>>.Success(new Dictionary<Guid, string>());
    }
}