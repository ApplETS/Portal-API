namespace WhatsNewApi.Models.Options
{
    public class FirebaseSettings : IFirebaseSettings
    {
        public string ProjectId { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
    }

    public interface IFirebaseSettings
    {
        string ProjectId { get; }
        string ApiKey { get; }
    }
}
