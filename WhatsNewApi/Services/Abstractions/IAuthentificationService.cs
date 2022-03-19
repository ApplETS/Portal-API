namespace WhatsNewApi.Services.Abstractions
{
    public interface IAuthentificationService
    {
        public Task<Models.Entities.User> Authenticate(string email, string password);
    }
}
