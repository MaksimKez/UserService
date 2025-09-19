using Domain.Results;

namespace Application.Services.Interfaces;

public interface INotificationFinalizer
{
    Task<Result<Dictionary<Guid, string>>> FinalizeAsync(int totalUsers, Dictionary<Guid, string> unnotifiedErrors, CancellationToken ct);
}
