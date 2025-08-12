using Application.Results;
using Domain.Entities;

namespace Application.Abstractions;

public interface IUserProfileService
{
    Task<Result<UserProfileEntity>> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    //todo i dont need methods related with specs, so i will make it later
    Task<Result<UserProfileEntity>> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<Guid> AddDefaultAsync(UserProfileEntity entity, CancellationToken cancellationToken);
    Task<Result> UpdateAsync(UserProfileEntity entity, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(Guid userId, CancellationToken cancellationToken);
}