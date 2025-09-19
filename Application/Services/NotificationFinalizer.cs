using Application.Abstractions;
using Domain.Abstractions;
using Domain.Results;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class NotificationFinalizer
    (IUnitOfWork uow,
    ILogger<NotificationFinalizer> logger)
    : INotificationFinalizer
{
    public async Task<Result<Dictionary<Guid, string>>> FinalizeAsync(
        int totalUsers,
        Dictionary<Guid, string> unnotifiedErrors,
        CancellationToken ct)
    {
        if (unnotifiedErrors.Count > 0)
        {
            logger.LogError("Some users have not been notified ({Failed}/{Total})",
                unnotifiedErrors.Count, totalUsers);
            await uow.SaveChangesAsync(ct);
            return Result<Dictionary<Guid, string>>.PartialFailure(
                unnotifiedErrors.Values.ToArray(),
                unnotifiedErrors);
        }

        logger.LogInformation("Notifications sent for {Count} users", totalUsers);
        await uow.SaveChangesAsync(ct);
        return Result<Dictionary<Guid, string>>.Success(new Dictionary<Guid, string>());
    }
}
