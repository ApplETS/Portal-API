namespace WhatsNewApi.Services.Abstractions;
public interface IFirebaseService
{
    public Task CreateUser(string email, string password, string role);
}
