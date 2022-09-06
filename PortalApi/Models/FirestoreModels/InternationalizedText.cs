using Google.Cloud.Firestore;

namespace PortalApi.Models.FirestoreModels;

[FirestoreData]
public class InternationalizedText
{
    [FirestoreProperty]
    public string? Fr { get; set; }
    
    [FirestoreProperty]
    public string? En { get; set; }
}

