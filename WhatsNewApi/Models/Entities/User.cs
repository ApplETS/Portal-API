namespace WhatsNewApi.Models.Entities;
public class User
{
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FirebaseToken { get; set; }
    public string? RefreshToken { get; set; }
}

