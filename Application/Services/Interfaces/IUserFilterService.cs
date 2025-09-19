using Application.Dtos;
using Application.Dtos.Requests;
using Domain.Entities;
using Domain.Results;

namespace Application.Services.Interfaces;

public interface IUserFilterService
{
    Task<Result<UserFilterEntity>> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Result<UserFilterEntity>> GetByProfileIdAsync(Guid profileId, CancellationToken ct);
    Task<Result<List<UserFilterEntity>>> ListAsync(ListingDto listing, CancellationToken ct);
    Task<Guid> AddDefaultAsync(Guid userId, CancellationToken ct);
    Task<Result> UpdateAsync(UpdateFilterRequest request, CancellationToken ct);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct);
}
