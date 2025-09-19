using Application.Dtos;

namespace Infrastructure.AuthServiceClient;

public class UserListingPairDto
{
    public UserDto User { get; set; }
    public ListingDto Listing { get; set; }
}
