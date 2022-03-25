using Google.Cloud.Firestore;
using WhatsNewApi.Extensions;
using WhatsNewApi.Models.Exceptions;
using WhatsNewApi.Models.FirestoreModels;
using WhatsNewApi.Models.Options;
using WhatsNewApi.Repos.Abstractions;

namespace WhatsNewApi.Repos;
public class ProjectRepository : IProjectRepository
{
    private readonly ILogger<ProjectRepository> _logger;
    private readonly CollectionReference _projectCollection;

    public ProjectRepository(ILogger<ProjectRepository> logger, IFirebaseSettings settings)
	{
		FirestoreDb db = FirestoreDb.Create(settings.ProjectId);
        _projectCollection = db.Collection("projects");
        _logger = logger;
    }

    public async Task Create(Project project)
    { 
        await _projectCollection.AddAsync(project);
    }

	public async Task Delete(string id)
    {
        try
        {
            DocumentSnapshot? document = await GetProjectDocument(id);
            if(document != null) await document.Reference.DeleteAsync();
            else throw new FirebaseException($"Getting project with id {id} has failed");
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            throw new FirebaseException($"Deleting a project has failed with the following: {ex.Message}");
        }
    }

    public async Task<Project> Get(string id)
    {
        var document = await GetProjectDocument(id);
        if (document != null) return document.ConvertTo<Project>();
        else throw new FirebaseException($"Getting project with id {id} has failed");
    }

    private async Task<DocumentSnapshot?> GetProjectDocument(string id)
    {
        var snapshot = await _projectCollection.GetSnapshotAsync();
        return snapshot.Documents.FirstOrDefault(doc => doc.Id == id);
    }

    public async Task<IEnumerable<Project>> GetAll()
    {
        var snapshot = await _projectCollection.GetSnapshotAsync();
        return snapshot.Documents.Select(document => document.ConvertTo<Project>());
    }

    public async Task Update(Project project)
    {
        if (!string.IsNullOrEmpty(project.Id))
        {
            var document = await GetProjectDocument(project.Id);
            if (document != null) await document.Reference.SetAsync(project);
            else throw new FirebaseException($"Getting project with id {project.Id} has failed");
        }
        else
        {
            throw new FirebaseException($"Getting project with id {project.Id} has failed");
        }
    }
}
