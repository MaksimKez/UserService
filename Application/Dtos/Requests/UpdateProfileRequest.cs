namespace Application.Dtos.Requests;

public class UpdateProfileRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string PreferredLanguage { get; set; } = "EN";
}