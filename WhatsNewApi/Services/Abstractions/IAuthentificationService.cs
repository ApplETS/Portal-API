namespace WhatsNewApi.Services.Abstractions;
public interface IAuthentificationService
{
    public Task<Models.Entities.User> Authenticate(string email, string password);
    public Task<Models.Entities.User> RefreshAuth(string accessToken, string refreshToken);
}

