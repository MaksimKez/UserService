using Application.Abstractions;
using Application.Results;
using Domain.Abstractions;
using Domain.Entities;

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

    public async Task<Guid> AddDefaultAsync(UserProfileEntity entity, CancellationToken cancellationToken)
    {
        entity.PreferredLanguage = "EN";
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        
        await uow.UserProfiles.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public async Task<Result> UpdateAsync(UserProfileEntity entity, CancellationToken cancellationToken)
    {
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