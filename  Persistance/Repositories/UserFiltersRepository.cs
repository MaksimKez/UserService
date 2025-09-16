using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories;

public class UserFiltersRepository(
    UserServiceDbContext context,
    ILogger<UserFiltersRepository> logger)
    : IUserFiltersRepository
{
    private readonly DbSet<UserFilterEntity> filters = context.Set<UserFilterEntity>();

    public async Task<UserFilterEntity?> GetByProfileIdAsync(Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting UserFilterEntity by ProfileId {ProfileId}", id);
        var entity = await filters.Include(f => f.Profile).FirstOrDefaultAsync(filter => filter.ProfileId == id, cancellationToken);
        if (entity == null)
            logger.LogWarning("UserFilterEntity with ProfileId {ProfileId} not found", id);
        return entity;
    }

    public async Task<UserFilterEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting UserFilterEntity by Id {Id}", id);
        var entity = await filters.FindAsync([id], cancellationToken);
        if (entity == null)
            logger.LogWarning("UserFilterEntity with Id {Id} not found", id);
        return entity;
    }

    public async Task<UserFilterEntity?> GetBySpecAsync(
        ISpecification<UserFilterEntity> specification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting UserFilterEntity by specification");
        var entity = await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
            logger.LogWarning("UserFilterEntity matching specification not found");
        return entity;
    }

    public async Task<List<UserFilterEntity>> ListAsync(
        ISpecification<UserFilterEntity> specification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Listing UserFilterEntities by specification");
        return await ApplySpecification(specification)
            .Include(f => f.Profile)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public IAsyncEnumerable<UserFilterEntity> StreamAsync(ISpecification<UserFilterEntity> specification)
    {
        logger.LogInformation("Streaming UserProfileEntities by specification");
        var filtersAsyncEnumerable =  ApplySpecification(specification)
            .Include(f => f.Profile)
            .AsNoTracking()
            .AsAsyncEnumerable();
        return filtersAsyncEnumerable;
    }

    private IQueryable<UserFilterEntity> ApplySpecification(ISpecification<UserFilterEntity> spec)
        => SpecificationEvaluator.Default.GetQuery(filters.AsQueryable(), spec);
    
    public async Task AddAsync(UserFilterEntity entity, CancellationToken cancellationToken)
    {
        logger.LogInformation("Adding new UserFilterEntity with ProfileId {ProfileId}", entity.ProfileId);
        await filters.AddAsync(entity, cancellationToken);
    }
    
    public void Update(UserFilterEntity entity)
    {
        logger.LogInformation("Updating UserFilterEntity with Id {Id}", entity.Id);
        filters.Update(entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting UserFilterEntity with Id {Id}", id);
        var entity = await filters.FindAsync(new object[] { id }, cancellationToken);
        if (entity is null)
        {
            logger.LogWarning("Attempt to delete UserFilterEntity with Id {Id}, but it was not found.", id);
            return;
        }

        filters.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        logger.LogInformation("UserFilterEntity with Id {Id} was successfully deleted.", id);
    }
}