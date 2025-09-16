using Domain.Results;
using Refit;
namespace Infrastructure.AuthServiceClient.Contracts.Interfaces;

public interface INotificationServiceApi
{
    [Post("/notify-single")]
    Task<HttpResponseMessage> NotifySingle([Body] UserListingPairDto userListingPair);
    
    [Post("/notify-multiple")]
    Task<HttpResponseMessage> NotifyMultiple([Body] UserListingPairDto[] userListingPairs);
}