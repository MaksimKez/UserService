using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IUserProfileRepository
{
    Task<UserProfileEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserProfileEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    
    Task<UserProfileEntity?> GetBySpecAsync(
        ISpecification<UserProfileEntity> specification, 
        CancellationToken cancellationToken = default);

    Task<List<UserProfileEntity>> ListAsync(
        ISpecification<UserProfileEntity> specification,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<UserProfileEntity> StreamAsync(ISpecification<UserProfileEntity> specification);

    Task AddAsync(UserProfileEntity entity, CancellationToken cancellationToken = default);

    void Update(UserProfileEntity entity);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}