namespace Application.Dtos.Requests;

public class AddUserProfileRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? PreferredLanguage { get; set; } = "EN";
}
