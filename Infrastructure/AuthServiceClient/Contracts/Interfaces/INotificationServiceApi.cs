using Domain.Results;
using Refit;
namespace Infrastructure.AuthServiceClient.Contracts.Interfaces;

public interface INotificationServiceApi
{
    [Post("notify-single")]
    Task<NotificationResult> NotifySingle([Body] UserListingPairDto userListingPair);
    
    [Post("notify-multiple")]
    Task<NotificationResult> NotifyMultiple([Body] UserListingPairDto[] userListingPairs);
}