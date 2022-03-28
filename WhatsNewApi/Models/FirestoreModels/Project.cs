using Google.Cloud.Firestore;

namespace WhatsNewApi.Models.FirestoreModels;
[FirestoreData]
public class Project
{
    [FirestoreDocumentId]
    public string? Id { get; set; }
    [FirestoreProperty]
    public string? Name { get; set; }
    [FirestoreProperty]
    public string? CurrentVersion { get; set; }
}
