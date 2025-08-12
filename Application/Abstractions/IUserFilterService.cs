using Application.Dtos;
using Application.Dtos.Requests;
using Application.Results;
using Domain.Entities;

namespace Application.Abstractions;

public interface IUserFilterService
{
    Task<Result<UserFilterEntity>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<UserFilterEntity>> GetByProfileIdAsync(Guid profileId, CancellationToken cancellationToken);
    Task<Result<List<UserFilterEntity>>> ListAsync(
        ListingDto matchingListing,
        CancellationToken cancellationToken = default);
    
    Task<Result<Dictionary<Guid, string>>> NotifyUsersAsync(ListingDto matchingListing, CancellationToken cancellationToken = default);
    
    Task<Result<Dictionary<Guid, string>>> NotifyUsersAsync(List<ListingDto> matchingListings, CancellationToken cancellationToken = default);
    
    Task<Guid> AddDefaultAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<Result> Update(UpdateFilterRequest entity);

    Task<Result> DeleteAsync(Guid id);
}

//get
//set filters
// parse gateway sends post (with listing filters)
// -> user service finds matching filters (and users' ids)
// -> user service sends post (with ids[]) to auth service
// -> auth sends post to notification service (with emails/telegram ids [])
// -> notification service notify users