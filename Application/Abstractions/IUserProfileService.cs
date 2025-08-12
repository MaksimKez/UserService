using Application.Dtos.Requests;
using Domain.Entities;
using Domain.Results;

namespace Application.Abstractions;

public interface IUserProfileService
{
    Task<Result<UserProfileEntity>> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    //todo i dont need methods related with specs, so i will make it later
    Task<Result<UserProfileEntity>> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<Result<AddUserProfileRequest>> AddAsync(AddUserProfileRequest request, CancellationToken cancellationToken);
    Task<Result> UpdateAsync(UpdateProfileRequest request, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(Guid userId, CancellationToken cancellationToken);
}