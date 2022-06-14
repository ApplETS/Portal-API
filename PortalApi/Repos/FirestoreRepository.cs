using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using WhatsNewApi.Extensions;
using WhatsNewApi.Models.Exceptions;
using WhatsNewApi.Models.Options;
using WhatsNewApi.Repos.Abstractions;

namespace WhatsNewApi.Repos;
public class FirestoreRepository<T> : IFirestoreRepository<T>
{
    private readonly ILogger<FirestoreRepository<T>> _logger;
    private readonly FirestoreDb _db;
    private readonly CollectionReference _projectCollection;

    public FirestoreRepository(ILogger<FirestoreRepository<T>> logger, IFirebaseSettings settings)
	{
        var builder = new FirestoreClientBuilder
        {
            CredentialsPath = Path.Combine("local", "adminsdk.json")
        };
        _db = FirestoreDb.Create(settings.ProjectId, builder.Build());
        _projectCollection = _db.Collection($"{typeof(T).Name}s");
        _logger = logger;
    }

    public async Task Create(T project)
    { 
        await _projectCollection.AddAsync(project);
    }

	public async Task Delete(string id)
    {
        try
        {
            DocumentSnapshot? document = await GetDocumentFromId(id);
            if(document != null) await document.Reference.DeleteAsync();
            else throw new FirebaseException($"Getting {nameof(T)} with id {id} has failed");
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            throw new FirebaseException($"Deleting a {nameof(T)} has failed with the following: {ex.Message}");
        }
    }

    public async Task<T> Get(string id)
    {
        var document = await GetDocumentFromId(id);
        if (document != null) return document.ConvertTo<T>();
        else throw new FirebaseException($"Getting {nameof(T)} with id {id} has failed");
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        var snapshot = await _projectCollection.GetSnapshotAsync();
        return snapshot.Documents.Select(document => document.ConvertTo<T>());
    }

    public async Task Update(string id, T documentToUpdate)
    {
        if (!string.IsNullOrEmpty(id))
        {
            var document = await GetDocumentFromId(id);
            if (document != null) await document.Reference.SetAsync(documentToUpdate);
            else throw new FirebaseException($"Getting {nameof(T)} with id {id} has failed");
        }
        else
        {
            throw new FirebaseException($"Getting {nameof(T)} with id {id} has failed");
        }
    }

    private async Task<DocumentSnapshot?> GetDocumentFromId(string id)
    {
        var snapshot = await _projectCollection.GetSnapshotAsync();
        return snapshot.Documents.FirstOrDefault(doc => doc.Id == id);
    }
}
