using System.Security.AccessControl;
using Application.Abstractions;
using Application.Abstractions.AuthServiceClient;
using Application.Dtos;
using Application.Results;
using Application.Specifications;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class UserFilterService(
    IUnitOfWork uow,
    INotificationServiceClient notificationServiceClient,
    ILogger<UserFilterService> logger)
    : IUserFilterService
{
    public async Task<Result<UserFilterEntity>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting UserFilterEntity by Id {Id}", id);
        var entity = await uow.UserFilters.GetByIdAsync(id, cancellationToken);
        if (entity != null)
            return Result<UserFilterEntity>.Success(entity);
        
        logger.LogWarning("UserFilterEntity with Id {Id} not found", id);
        return Result<UserFilterEntity>.Failure("Not found");
    }

    public async Task<Result<UserFilterEntity>> GetByProfileIdAsync(Guid profileId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting UserFilterEntity by ProfileId {ProfileId}", profileId);
        var entity = await uow.UserFilters.GetByProfileIdAsync(profileId, cancellationToken);
        if (entity != null)
            return Result<UserFilterEntity>.Success(entity);
        
        logger.LogWarning("UserFilterEntity with ProfileId {ProfileId} not found", profileId);
        return Result<UserFilterEntity>.Failure("Not found");
    }

    public async Task<Result<List<UserFilterEntity>>> ListAsync(ListingDto matchingListing, CancellationToken ct)
    {
        logger.LogInformation("Listing UserFilterEntities by specification from ListingDto");

        var spec = new FilterSpecification(matchingListing);
        var entities = await uow.UserFilters.ListAsync(spec, ct);
        return Result<List<UserFilterEntity>>.Success(entities);
    }
    
    public async Task<Guid> AddAsync(UserFilterEntity entity)
    {
        logger.LogInformation("Adding new UserFilterEntity with ProfileId {ProfileId}", entity.ProfileId);
        await uow.UserFilters.AddAsync(entity, CancellationToken.None);
        await uow.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<Result> Update(UserFilterEntity entity)
    {
        logger.LogInformation("Updating UserFilterEntity with Id {Id}", entity.Id);
        uow.UserFilters.Update(entity);
        await uow.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        logger.LogInformation("Deleting UserFilterEntity with Id {Id}", id);
        await uow.UserFilters.DeleteAsync(id, CancellationToken.None);
        await uow.SaveChangesAsync();
        return Result.Success();
    }
    
    public async Task<Result<Dictionary<Guid, string>>> NotifyUsersAsync(ListingDto listing, CancellationToken cancellationToken)
    {
        logger.LogInformation("Notify users for a single ListingDto using streaming");

        var notifiedUsers = new HashSet<Guid>();
        var unnotifiedErrors = new Dictionary<Guid, string>();

        await foreach (var filter in uow.UserFilters.StreamAsync(new FilterSpecification(listing))
            .WithCancellation(cancellationToken))
        {
            if (filter.Profile.LastNotifiedAt.HasValue &&
                filter.Profile.LastNotifiedAt > DateTime.UtcNow.AddHours(-1))
            {
                continue;
            }

            if (!notifiedUsers.Add(filter.ProfileId))
                continue;

            logger.LogInformation("Notifying user with profile id {id}", filter.ProfileId);

            var result = await notificationServiceClient.NotifyUserAsync(new UserDto()
            {
                Id = filter.ProfileId,
                Email = filter.Profile.Email,
                Name = filter.Profile.FirstName,
                LastName = filter.Profile.LastName,
                TelegramId = null //temp
            }, listing, cancellationToken);

            if (result.IsSuccess)
            {
                filter.Profile.LastNotifiedAt = DateTime.UtcNow;
            }
            else
            {
                unnotifiedErrors[filter.ProfileId] = result.Errors?.FirstOrDefault() ?? "Unknown error";
                logger.LogError("Failed to notify user {id}: {error}", filter.ProfileId, unnotifiedErrors[filter.ProfileId]);
            }
        }

        return await FinalizeNotificationAsync(notifiedUsers.Count, unnotifiedErrors, cancellationToken);
    }

    public async Task<Result<Dictionary<Guid, string>>> NotifyUsersAsync(List<ListingDto> listings, CancellationToken cancellationToken)
    {
        logger.LogInformation("Notify users for multiple ListingDtos");

        var userToListing = new Dictionary<UserDto, ListingDto>();

        foreach (var listing in listings)
        {
            var filters = await uow.UserFilters.ListAsync(new FilterSpecification(listing), cancellationToken);

            foreach (var filter in filters)
            {
                if (filter.Profile.LastNotifiedAt.HasValue 
                    && filter.Profile.LastNotifiedAt > DateTime.UtcNow.AddHours(-1) )
                {
                    continue;
                }
                userToListing[new UserDto
                {
                    Id = filter.ProfileId,
                    Email = filter.Profile.Email,
                    Name = filter.Profile.FirstName,
                    LastName = filter.Profile.LastName,
                    TelegramId = null //temp
                }] = listing;
            }
        }

        if (userToListing.Count == 0)
        {
            logger.LogInformation("No users found to notify for listings");
            return Result<Dictionary<Guid, string>>.Success(new Dictionary<Guid, string>());
        }
        

        var sendResult = await notificationServiceClient.NotifyUsersAsync(userToListing, cancellationToken);
        var unnotifiedErrors = sendResult.IsSuccess ? new Dictionary<Guid, string>() : sendResult.Value ?? new();

        foreach (var (userId, error) in unnotifiedErrors)
        {
            logger.LogError("Failed to notify user {id}: {error}", userId, error);
        }

        if (sendResult.IsSuccess)
        {
            foreach (var user in userToListing.Keys)
            {
                var filter = await uow.UserFilters.GetByProfileIdAsync(user.Id, cancellationToken);
                if (filter != null)
                {
                    filter.Profile.LastNotifiedAt = DateTime.UtcNow;
                }
            }
        }

        return await FinalizeNotificationAsync(userToListing.Count, unnotifiedErrors, cancellationToken);
    }

    private async Task<Result<Dictionary<Guid, string>>> FinalizeNotificationAsync(
        int totalUsers,
        Dictionary<Guid, string> unnotifiedErrors,
        CancellationToken cancellationToken)
    {
        if (unnotifiedErrors.Count > 0)
        {
            logger.LogError("Some users have not been notified ({Failed}/{Total})",
                unnotifiedErrors.Count, totalUsers);
            await uow.SaveChangesAsync(cancellationToken);
            return Result<Dictionary<Guid, string>>.PartialFailure(
                unnotifiedErrors.Values.ToArray(),
                unnotifiedErrors);
        }

        logger.LogInformation("Notifications sent for {Count} users", totalUsers);
        await uow.SaveChangesAsync(cancellationToken);
        return Result<Dictionary<Guid, string>>.Success(new());
    }
}