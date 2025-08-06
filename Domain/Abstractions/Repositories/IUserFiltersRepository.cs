using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IUserFiltersRepository
{
    Task<UserFilterEntity?> GetByProfileIdAsync(Guid id);
    Task<UserFilterEntity?> GetByIdAsync(Guid id);
    
    Task<UserFilterEntity?> GetBySpecAsync(ISpecification<UserFilterEntity> specification);

    Task<List<UserFilterEntity>> ListAsync(ISpecification<UserFilterEntity> specification);

    Task AddAsync(UserFilterEntity entity);

    Task UpdateAsync(UserFilterEntity entity);

    Task DeleteAsync(UserFilterEntity entity);
}