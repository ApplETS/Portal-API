using Google.Cloud.Firestore;

namespace WhatsNewApi.Models.FirestoreModels;

[FirestoreData]
public class WhatsNewPage
{
    [FirestoreProperty]
    public string? Title { get; set; }
    [FirestoreProperty]
    public string? Description { get; set; }
    [FirestoreProperty]
    public string? MediaUrl { get; set; }
    // Color as hexadecimal value
    [FirestoreProperty]
    public string? Color { get; set; }
}