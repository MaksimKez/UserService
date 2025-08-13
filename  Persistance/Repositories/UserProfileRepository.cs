using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Results;
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

    public async Task<UserProfileEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting UserProfileEntity by Email {Email}", email);
        var entity = await profiles.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        if (entity == null) 
            logger.LogWarning("UserProfileEntity with Email {Email} not found", email);
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

    public async Task<Result<UserProfileEntity>> AddAsync(UserProfileEntity entity, CancellationToken cancellationToken = default)
    {
        var existingUser = await GetByEmailAsync(entity.Email, cancellationToken);
        if (existingUser != null)
        {
            return Result<UserProfileEntity>.Failure("User already exists");
        }
        
        logger.LogInformation("Adding new UserProfileEntity with Id {Id}", entity.Id);
        await profiles.AddAsync(entity, cancellationToken);
        return Result<UserProfileEntity>.Success(entity);
    }

    public void Update(UserProfileEntity entity)
    {
        logger.LogInformation("Updating UserProfileEntity with Id {Id}", entity.Id);
        profiles.Update(entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Deleting UserProfile with Id {Id}", id);
        var entity = await profiles.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            logger.LogWarning("Attempt to delete UserProfile with Id {Id}, but it was not found.", id);
            return;
        }

        profiles.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        logger.LogInformation("UserProfile with Id {Id} was successfully deleted.", id);

    }
}
