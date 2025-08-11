using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IUserFiltersRepository
{
    Task<UserFilterEntity?> GetByProfileIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserFilterEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<UserFilterEntity?> GetBySpecAsync(
        ISpecification<UserFilterEntity> specification, 
        CancellationToken cancellationToken = default);

    Task<List<UserFilterEntity>> ListAsync(
        ISpecification<UserFilterEntity> specification,
        CancellationToken cancellationToken = default);

    Task AddAsync(UserFilterEntity entity, CancellationToken cancellationToken = default);

    void Update(UserFilterEntity entity);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}