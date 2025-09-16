using Application.Abstractions;
using Application.Dtos;
using Application.Dtos.Requests;
using Application.Specifications;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Results;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class UserFilterService(
    IUnitOfWork uow,
    ILogger<UserFilterService> logger,
    IMapper mapper) 
    : IUserFilterService
{
    public async Task<Result<UserFilterEntity>> GetByIdAsync(Guid id, CancellationToken ct)
    {
        logger.LogInformation("Getting UserFilterEntity by Id {Id}", id);
        var entity = await uow.UserFilters.GetByIdAsync(id, ct);
        return entity != null
            ? Result<UserFilterEntity>.Success(entity)
            : Result<UserFilterEntity>.Failure("Not found");
    }

    public async Task<Result<UserFilterEntity>> GetByProfileIdAsync(Guid profileId, CancellationToken ct)
    {
        logger.LogInformation("Getting UserFilterEntity by ProfileId {ProfileId}", profileId);
        var entity = await uow.UserFilters.GetByProfileIdAsync(profileId, ct);
        return entity != null
            ? Result<UserFilterEntity>.Success(entity)
            : Result<UserFilterEntity>.Failure("Not found");
    }

    public async Task<Result<List<UserFilterEntity>>> ListAsync(ListingDto listing, CancellationToken ct)
    {
        logger.LogInformation("Listing UserFilterEntities by specification from ListingDto");
        var spec = new FilterSpecification(listing);
        var entities = await uow.UserFilters.ListAsync(spec, ct);
        return Result<List<UserFilterEntity>>.Success(entities);
    }

    public async Task<Guid> AddDefaultAsync(Guid userId, CancellationToken ct)
    {
        var entity = UserFilterEntity.Default(userId);
        logger.LogInformation("Adding new UserFilterEntity with ProfileId {ProfileId}", entity.ProfileId);
        await uow.UserFilters.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return entity.Id;
    }

    public async Task<Result> UpdateAsync(UpdateFilterRequest request, CancellationToken ct)
    {
        logger.LogInformation("Updating UserFilterEntity with Id {Id}", request.Id);
        var entity = mapper.Map<UserFilterEntity>(request);
        uow.UserFilters.Update(entity);
        await uow.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct)
    {
        logger.LogInformation("Deleting UserFilterEntity with Id {Id}", id);
        await uow.UserFilters.DeleteAsync(id, ct);
        await uow.SaveChangesAsync(ct);
        return Result.Success();
    }
}
