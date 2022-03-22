using Firebase.Auth;
using WhatsNewApi.Extensions;
using WhatsNewApi.Models.Exceptions;
using WhatsNewApi.Models.Options;
using WhatsNewApi.Services.Abstractions;

namespace WhatsNewApi.Services;
public class AuthentificationService : IAuthentificationService
{
    private readonly FirebaseAuthProvider _authProvider;
    private readonly ILogger _logger;

    public AuthentificationService(IFirebaseSettings settings, ILogger<AuthentificationService> logger)
    {
        _authProvider = new FirebaseAuthProvider(new FirebaseConfig(settings.ApiKey));
        _logger = logger;
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
            _logger.LogException(ex);
            throw new AuthException($"Sign in with email and password has failed with the following: {ex.Message}");
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
            _logger.LogException(ex);
            throw new AuthException($"Refresh auth has failed with the following: {ex.Message}. There might be an issue with the headers sent or the Firebase API Token");
        }
    }
}

