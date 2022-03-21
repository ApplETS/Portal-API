using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using WhatsNewApi.Services.Abstractions;

namespace WhatsNewApi.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseAuth _client;
        public FirebaseService()
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),
            });
            _client = FirebaseAuth.DefaultInstance;
        }

        public async Task<bool> CreateUser(string email, string password, string role)
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
                return false;
            }

            return true;
        }
    }
}
