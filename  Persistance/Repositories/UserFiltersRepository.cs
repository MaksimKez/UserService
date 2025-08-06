using Ardalis.Specification;
using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Persistence.Repositories;

public class UserFiltersRepository : IUserFiltersRepository
{
    public Task<UserFilterEntity?> GetByProfileIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<UserFilterEntity?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<UserFilterEntity?> GetBySpecAsync(ISpecification<UserFilterEntity> specification)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserFilterEntity>> ListAsync(ISpecification<UserFilterEntity> specification)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(UserFilterEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(UserFilterEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(UserFilterEntity entity)
    {
        throw new NotImplementedException();
    }
}