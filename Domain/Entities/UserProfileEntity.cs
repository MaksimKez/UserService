using System.Security.AccessControl;

namespace Domain.Entities;

public class UserProfileEntity
{
    /// <summary>
    /// Id MUST be the same as in AuthService
    /// </summary>
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? ImageUrl { get; set; }
    public string? PreferredLanguage { get; set; }
    
    public UserFilterEntity Filter { get; set; }
    
    public DateTime? LastNotifiedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}