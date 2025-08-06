using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Microsoft.Extensions.Logging;

namespace Persistence;

public class UnitOfWork(
    UserServiceDbContext context,
    IUserFiltersRepository userFilters,
    IUserProfileRepository userProfiles,
    ILogger<UnitOfWork> logger)
    : IUnitOfWork
{
    public IUserFiltersRepository UserFilters { get; } = userFilters;
    public IUserProfileRepository UserProfiles { get; } = userProfiles;

    private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Saving changes to the database.");
        var result = await context.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Changes saved: {Count} entries affected.", result);
        return result;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (HasActiveTransaction)
        {
            logger.LogWarning("Attempted to begin a new transaction while another is active.");
            return new UserServiceDbTransaction(_currentTransaction!);
        }

        logger.LogInformation("Beginning new transaction.");
        _currentTransaction = await context.Database.BeginTransactionAsync(cancellationToken);
        logger.LogInformation("Transaction started.");
        return new UserServiceDbTransaction(_currentTransaction);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (!HasActiveTransaction)
        {
            logger.LogError("Attempted to commit with no active transaction.");
            throw new InvalidOperationException("No active transaction");
        }

        logger.LogInformation("Committing transaction.");
        await _currentTransaction!.CommitAsync(cancellationToken);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
        logger.LogInformation("Transaction committed.");
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (!HasActiveTransaction)
        {
            logger.LogError("Attempted to rollback with no active transaction.");
            throw new InvalidOperationException("No active transaction");
        }

        logger.LogWarning("Rolling back transaction.");
        await _currentTransaction!.RollbackAsync(cancellationToken);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
        logger.LogInformation("Transaction rolled back.");
    }

    public void Dispose()
    {
        logger.LogInformation("Disposing UnitOfWork.");
        context.Dispose();
    }

}