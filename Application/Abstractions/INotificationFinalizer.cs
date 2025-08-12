using Application.Results;

namespace Application.Abstractions;

public interface INotificationFinalizer
{
    Task<Result<Dictionary<Guid, string>>> FinalizeAsync(int totalUsers, Dictionary<Guid, string> unnotifiedErrors, CancellationToken ct);
}
