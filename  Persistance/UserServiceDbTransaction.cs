using Domain.Abstractions;

namespace Persistence;

public class UserServiceDbTransaction(
    Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction efTransaction)
    : IDbContextTransaction
{
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await efTransaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default) => await efTransaction.RollbackAsync(cancellationToken);

    public void Dispose() => efTransaction.Dispose();
}