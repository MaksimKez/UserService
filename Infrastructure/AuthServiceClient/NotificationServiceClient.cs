using Application.Abstractions.NotificationServiceClient;
using Application.Dtos;
using Domain.Results;

namespace Infrastructure.AuthServiceClient;

public class NotificationServiceClient : INotificationServiceClient
{

    public async Task<Result> NotifyUserAsync(UserDto dto, ListingDto matchingListing, CancellationToken cancellationToken = default)
    {
        Print(dto);
        await Task.Delay(1000, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<Dictionary<Guid, string>>> NotifyUsersAsync(Dictionary<UserDto, ListingDto> usersToListingDtos, CancellationToken cancellationToken = default)
    {
        foreach (var userToListingDto in usersToListingDtos)
        {
            Print(userToListingDto.Key);
        }
        await Task.Delay(1000, cancellationToken);
        return Result<Dictionary<Guid, string>>.Success(new Dictionary<Guid, string>());
    }

    private void Print(UserDto userDto)
    {
        Console.WriteLine("\n");
        Console.WriteLine($"Name: {userDto.Name}", ConsoleColor.Green);
        Console.WriteLine($"Email: {userDto.Email}", ConsoleColor.Green);
        Console.WriteLine("\n");
    }
}