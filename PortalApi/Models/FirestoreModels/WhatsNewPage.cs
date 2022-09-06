using Google.Cloud.Firestore;
using PortalApi.Models.FirestoreModels;

namespace WhatsNewApi.Models.FirestoreModels;

[FirestoreData]
public class WhatsNewPage
{
    [FirestoreProperty]
    public InternationalizedText? Title { get; set; }
    [FirestoreProperty]
    public InternationalizedText? Description { get; set; }
    [FirestoreProperty]
    public string? MediaUrl { get; set; }
    // Color as hexadecimal value
    [FirestoreProperty]
    public string? Color { get; set; }
}