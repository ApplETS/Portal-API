using System;
using Google.Cloud.Firestore;

namespace WhatsNewApi.Models.FirestoreModels;

[FirestoreData]
public class WhatsNew
{
    [FirestoreDocumentId]
    public string? Id { get; set; }
    [FirestoreProperty]
    public string? ProjectId { get; set; }
    [FirestoreProperty]
    public string? Version { get; set; }
    [FirestoreProperty]
    public List<WhatsNewPage>? Pages { get; set; }
}
