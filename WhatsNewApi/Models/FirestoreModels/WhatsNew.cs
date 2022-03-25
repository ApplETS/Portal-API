using System;
using Google.Cloud.Firestore;

namespace WhatsNewApi.Models.FirestoreModels;

[FirestoreData]
public class WhatsNew
{
    [FirestoreProperty]
    public string Version { get; set; } = string.Empty;
    [FirestoreProperty]
    public List<WhatsNewPage>? Pages { get; set; }
}
