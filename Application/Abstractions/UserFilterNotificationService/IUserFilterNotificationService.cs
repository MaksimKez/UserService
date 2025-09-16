using Application.Dtos;
using Domain.Results;

namespace Application.Abstractions.UserFilterNotificationService;

public interface IUserFilterNotificationService
{
    Task<Result<Dictionary<Guid, string>>> NotifyUsersAsync(ListingDto listing, CancellationToken ct);
    Task<Result<Dictionary<Guid, string>>> NotifyUsersAsync(List<ListingDto> listings, CancellationToken ct);
}