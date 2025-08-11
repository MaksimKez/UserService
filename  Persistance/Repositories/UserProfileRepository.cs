using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories;

public class UserProfileRepository(UserServiceDbContext context, ILogger<UserProfileRepository> logger)
    : IUserProfileRepository
{
    private readonly DbSet<UserProfileEntity> profiles = context.Set<UserProfileEntity>();

    public async Task<UserProfileEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting UserProfileEntity by Id {Id}", id);
        var entity = await profiles.FindAsync([id], cancellationToken);
        if (entity == null)
            logger.LogWarning("UserProfileEntity with Id {Id} not found", id);
        return entity;
    }
    public async Task<UserProfileEntity?> GetBySpecAsync(
        ISpecification<UserProfileEntity> specification,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting UserProfileEntity by specification");
        var entity = await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
            logger.LogWarning("UserProfileEntity matching specification not found");
        return entity;
    }

    public async Task<List<UserProfileEntity>> ListAsync(ISpecification<UserProfileEntity> specification, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Listing UserProfileEntities by specification");
        return await ApplySpecification(specification)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public IAsyncEnumerable<UserProfileEntity> StreamAsync(ISpecification<UserProfileEntity> specification)
    {
        logger.LogInformation("Streaming UserProfileEntities by specification");
        return ApplySpecification(specification)
            .AsNoTracking()
            .AsAsyncEnumerable();
    }


    private IQueryable<UserProfileEntity> ApplySpecification(ISpecification<UserProfileEntity> spec)
        => SpecificationEvaluator.Default.GetQuery(profiles.AsQueryable(), spec);

    public async Task AddAsync(UserProfileEntity entity, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Adding new UserProfileEntity with Id {Id}", entity.Id);
        await profiles.AddAsync(entity, cancellationToken);
    }

    public void Update(UserProfileEntity entity)
    {
        logger.LogInformation("Updating UserProfileEntity with Id {Id}", entity.Id);
        profiles.Update(entity);
    }

    public async Task DeleteAsync(UserProfileEntity entity, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Deleting UserProfileEntity with Id {Id}", entity.Id);
        profiles.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        logger.LogInformation("UserProfileEntity with Id {Id} was successfully deleted.", entity.Id);
    }
}
