using Application.Abstractions;
using Application.Dtos.Requests;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Results;

namespace Application.Services;

public class UserProfileService(IUnitOfWork uow) : IUserProfileService
{
    public async Task<Result<UserProfileEntity>> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var entity = await uow.UserProfiles.GetByIdAsync(userId, cancellationToken);
        return entity is null ?
            Result<UserProfileEntity>.Failure("User not found") 
            : Result<UserProfileEntity>.Success(entity);
    }

    public async Task<Result<UserProfileEntity>> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        var entity = await uow.UserProfiles.GetByEmailAsync(email, ct);
        return entity is null ?
            Result<UserProfileEntity>.Failure("User not found") 
            : Result<UserProfileEntity>.Success(entity);
    }

    public async Task<Result<AddUserProfileRequest>> AddAsync(AddUserProfileRequest request, CancellationToken cancellationToken)
    {
        var entity = new UserProfileEntity
        {
            Id = request.Id,
            PreferredLanguage = request.PreferredLanguage,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            FirstName = request.Name,
            LastName = request.LastName,
            Email = request.Email,
        };

        var result = await uow.UserProfiles.AddAsync(entity, cancellationToken);
        if (!result.IsSuccess)
        {
            return Result<AddUserProfileRequest>.Failure(result.Errors!);
        }
        await uow.SaveChangesAsync(cancellationToken);
        
        return Result<AddUserProfileRequest>.Success(request);
    }

    public async Task<Result> UpdateAsync(UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var entity = await uow.UserProfiles.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            return Result.Failure("User not found");
        }
        
        entity.FirstName = request.Name;
        entity.LastName = request.LastName;
        entity.PreferredLanguage = request.PreferredLanguage;
        
        uow.UserProfiles.Update(entity);
        await uow.SaveChangesAsync(cancellationToken);
        return Result.Success();

    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await uow.UserProfiles.DeleteAsync(id, CancellationToken.None);
        await uow.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}