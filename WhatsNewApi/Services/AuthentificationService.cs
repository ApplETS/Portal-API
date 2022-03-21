using Firebase.Auth;
using WhatsNewApi.Models.Options;
using WhatsNewApi.Services.Abstractions;

namespace WhatsNewApi.Services;
public class AuthentificationService : IAuthentificationService
{
    private readonly FirebaseAuthProvider _authProvider;

    public AuthentificationService(IFirebaseSettings settings)
    {
        _authProvider = new FirebaseAuthProvider(new FirebaseConfig(settings.ApiKey));
    }

    public async Task<Models.Entities.User> Authenticate(string email, string password)
    {
        try
        {
            var authInfo = await _authProvider.SignInWithEmailAndPasswordAsync(email, password);
            return new Models.Entities.User
            { 
                Email = authInfo.User.Email,
                FirebaseToken = authInfo.FirebaseToken,
                RefreshToken = authInfo.RefreshToken
            };
                
        }
        catch (Exception ex)
        {
            // Invalid credentials
            throw new NotImplementedException(ex.Message);
        }
    }

    public async Task<Models.Entities.User> RefreshAuth(string accessToken, string refreshToken)
    {
        try
        {
            var authInfo = await _authProvider.RefreshAuthAsync(new FirebaseAuth() { FirebaseToken = accessToken, RefreshToken = refreshToken });
            return new Models.Entities.User
            {
                Email = authInfo.User.Email,
                FirebaseToken = authInfo.FirebaseToken,
                RefreshToken = authInfo.RefreshToken
            };
        }
        catch (Exception ex)
        {
            // Invalid credentials
            throw new NotImplementedException(ex.Message);
        }
    }
}

