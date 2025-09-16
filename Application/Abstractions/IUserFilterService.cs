using Application.Dtos;
using Application.Dtos.Requests;
using Domain.Entities;
using Domain.Results;

namespace Application.Abstractions;

public interface IUserFilterService
{
    Task<Result<UserFilterEntity>> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Result<UserFilterEntity>> GetByProfileIdAsync(Guid profileId, CancellationToken ct);
    Task<Result<List<UserFilterEntity>>> ListAsync(ListingDto listing, CancellationToken ct);
    Task<Guid> AddDefaultAsync(Guid userId, CancellationToken ct);
    Task<Result> UpdateAsync(UpdateFilterRequest request, CancellationToken ct);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct);
}

//get
//set filters
// parse gateway sends post (with listing filters)
// -> user service finds matching filters (and users' ids)
// -> user service sends post (with ids[]) to auth service
// -> auth sends post to notification service (with emails/telegram ids [])
// -> notification service notify users