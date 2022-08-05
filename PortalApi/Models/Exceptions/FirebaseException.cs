namespace WhatsNewApi.Models.Exceptions
{
    public class FirebaseException : Exception
    {
        public FirebaseException(string message) : base(message) { }
    }
}
