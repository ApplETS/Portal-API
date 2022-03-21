namespace WhatsNewApi.Services.Abstractions;
public interface IFirebaseService
{
    public Task<bool> CreateUser(string email, string password, string role);
}
