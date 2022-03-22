using Google.Cloud.Firestore;
using WhatsNewApi.Models.FirestoreModels;
using WhatsNewApi.Models.Options;
using WhatsNewApi.Repos.Abstractions;

namespace WhatsNewApi.Repos;
public class ProjectRepository : IProjectRepository
{
	private readonly FirestoreDb _db;
	public ProjectRepository(IFirebaseSettings settings)
	{
		_db = FirestoreDb.Create(settings.ProjectId);
	}

    public async Task Create(string projectName, string projectVersion)
    {
        try
        {
            CollectionReference projectCollection = _db.Collection("projects");
            await projectCollection.AddAsync(new Project { CurrentVersion = projectVersion, Name = projectName });
        }
        catch (Exception ex)
        {
            throw new Exception($"Creating a project has failed with the following: {ex.Message}");
        }
		
    }

	public async Task Delete(string id)
    {
        try
        {
            DocumentSnapshot? document = await GetProjectDocument(id);
            if(document != null) await document.Reference.DeleteAsync();
            else throw new Exception($"Getting project with id {id} has failed");
        }
        catch (Exception ex)
        {
            throw new Exception($"Deleting a project has failed with the following: {ex.Message}");
        }
    }

    public async Task<Project> Get(string id)
    {
        try
        {
            DocumentSnapshot? document = await GetProjectDocument(id);
            if (document != null) return document.ConvertTo<Project>();
            else throw new Exception($"Getting project with id {id} has failed");
        }
        catch (Exception ex)
        {
            throw new Exception($"Getting project with id {id} has failed with the following: {ex.Message}");
        }
    }

    private async Task<DocumentSnapshot?> GetProjectDocument(string id)
    {
        CollectionReference collection = _db.Collection("projects");
        var snapshot = await collection.GetSnapshotAsync();
        return snapshot.Documents.FirstOrDefault(doc => doc.Id == id);
    }

    public async Task<IEnumerable<Project>> GetAll()
    {
        try
        {
            CollectionReference collection = _db.Collection("projects");
            var snapshot = await collection.GetSnapshotAsync();
            return snapshot.Documents.Select(document => document.ConvertTo<Project>());
        }
        catch (Exception ex)
        {
            throw new Exception($"Getting all projects has failed with the following: {ex.Message}");
        }
    }

    public async Task UpdateVersion(string id, string version)
    {
        try
        {
            var document = await GetProjectDocument(id);
            if(document != null)
            {
                var project = document.ConvertTo<Project>();
                project.CurrentVersion = version;
                await document.Reference.SetAsync(project);
            }
            else
            {
                throw new Exception($"Creating a user has failed with the following: Id({id}) doesn't exist");
            }
            
        }
        catch (Exception ex)
        {
            throw new NotImplementedException($"Creating a user has failed with the following: {ex.Message}");
        }
    }

    public async Task AddWhatsNew(string id, string version, IEnumerable<WhatsNewPage> pages)
    {
        try
        {
            var document = await GetProjectDocument(id);
            if (document != null)
            {
                var project = document.ConvertTo<Project>();
                var whatsnew = new WhatsNew
                {
                    Version = version,
                    Pages = pages.ToList()
                };
                project.WhatsNews.Add(whatsnew);
                await document.Reference.SetAsync(project);
            }
            else
            {
                throw new Exception($"Creating a user has failed with the following: Id({id}) doesn't exist");
            }

        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
}
