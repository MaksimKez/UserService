using Application.Abstractions;
using Application.Abstractions.NotificationServiceClient;
using Application.Abstractions.UserFilterNotificationService;
using Application.Dtos;
using Application.Services.Interfaces;
using Application.Specifications;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Results;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class UserFilterNotificationService(
    IUnitOfWork uow,
    INotificationServiceClient notificationServiceClient,
    INotificationFinalizer finalizer,
    ILogger<UserFilterNotificationService> logger) : IUserFilterNotificationService
{
    public async Task<Result<Dictionary<Guid, string>>> NotifyUsersAsync(ListingDto listing, CancellationToken ct)
    {
        var notifiedUsers = new HashSet<Guid>();
        var unnotifiedErrors = new Dictionary<Guid, string>();

        await foreach (var filter in uow.UserFilters.StreamAsync(new FilterSpecification(listing))
            .WithCancellation(ct))
        {
            if (!CanNotify(filter))
            {
                logger.LogWarning("User was notified less than an hour ago");
                continue;
            }
            
            if (!notifiedUsers.Add(filter.ProfileId))
            {
                logger.LogWarning("User is in notifiedUsers already");
                continue;
            }
            logger.LogInformation("Notifying user with profile id {id}", filter.ProfileId);

            var result = await notificationServiceClient.NotifyUserAsync(ToUserDto(filter), listing, ct);

            if (result.IsSuccess)
                filter.Profile.LastNotifiedAt = DateTime.UtcNow;
            else
                LogErrorAndSaveToDictionary(unnotifiedErrors, filter.ProfileId, result);
        }

        return await finalizer.FinalizeAsync(notifiedUsers.Count, unnotifiedErrors, ct);
    }

    public async Task<Result<Dictionary<Guid, string>>> NotifyUsersAsync(List<ListingDto> listings, CancellationToken ct)
    {
        logger.LogInformation("Notify users for multiple ListingDtos");

        var userToListing = new Dictionary<UserDto, ListingDto>();

        foreach (var listing in listings)
        {
            var filters = await uow.UserFilters.ListAsync(new FilterSpecification(listing), ct);

            foreach (var filter in filters.Where(CanNotify))
            {
                userToListing[ToUserDto(filter)] = listing;
            }
        }

        if (userToListing.Count == 0)
            return Result<Dictionary<Guid, string>>.Success(new Dictionary<Guid, string>());

        var sendResult = await notificationServiceClient.NotifyUsersAsync(userToListing, ct);
        var unnotifiedErrors = sendResult.IsSuccess ? new Dictionary<Guid, string>() : sendResult.Value ?? new();

        foreach (var (userId, error) in unnotifiedErrors)
            logger.LogError("Failed to notify user {id}: {error}", userId, error);

        if (sendResult.IsSuccess)
        {
            foreach (var user in userToListing.Keys)
            {
                var filter = await uow.UserFilters.GetByProfileIdAsync(user.Id, ct);
                
                if (filter != null)
                    filter.Profile.LastNotifiedAt = DateTime.UtcNow;
            }

            await uow.SaveChangesAsync(ct);
        }

        return await finalizer.FinalizeAsync(userToListing.Count, unnotifiedErrors, ct);
    }

    private static bool CanNotify(UserFilterEntity filter)
    {
        if (!filter.Profile.LastNotifiedAt.HasValue)
        {
            filter.Profile.LastNotifiedAt = DateTime.UtcNow;
            return true;
        }

        return !(filter.Profile.LastNotifiedAt > DateTime.UtcNow.AddHours(-1));
    }
    private static UserDto ToUserDto(UserFilterEntity filter) => new()
    {
        Id = filter.ProfileId,
        Email = filter.Profile.Email,
        Name = filter.Profile.FirstName,
        LastName = filter.Profile.LastName,
        TelegramId = null
    };

    private void LogErrorAndSaveToDictionary(Dictionary<Guid, string> errors, Guid profileId, Result result)
    {
        var errorMsg = result.Errors?.FirstOrDefault() ?? "Unknown error";
        errors[profileId] = errorMsg;
        logger.LogError("Failed to notify user {id}: {error}", profileId, errorMsg);
    }
}
