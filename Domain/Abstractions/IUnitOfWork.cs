using Domain.Abstractions.Repositories;

namespace Domain.Abstractions;

public interface IUnitOfWork
{ 
    IUserFiltersRepository UserFilters { get; }
    IUserProfileRepository UserProfiles { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    
    bool HasActiveTransaction { get; }
}