using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using WhatsNewApi.Extensions;
using WhatsNewApi.Models.Exceptions;
using WhatsNewApi.Services.Abstractions;

namespace WhatsNewApi.Services;

public class FirebaseService : IFirebaseService
{
    private readonly FirebaseAuth _client;
    private readonly ILogger _logger;
    public FirebaseService(ILogger<FirebaseService> logger)
    {
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.GetApplicationDefault(),
        });
        _client = FirebaseAuth.DefaultInstance;
        _logger = logger;
    }

    public async Task CreateUser(string email, string password, string role)
    {
        try
        {
            var result = await _client.CreateUserAsync(new UserRecordArgs { Email = email, Password = password });
            await _client.SetCustomUserClaimsAsync(result.Uid, new Dictionary<string, object>()
            {
                { "role", role }
            });
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            throw new AuthException($"Creating a user has failed with the following: {ex.Message}");
        }
    }
}