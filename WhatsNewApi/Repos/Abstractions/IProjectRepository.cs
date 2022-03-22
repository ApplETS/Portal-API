using WhatsNewApi.Models.FirestoreModels;

namespace WhatsNewApi.Repos.Abstractions;

public interface IProjectRepository
{
	public Task Create(string projectName, string projectVersion);
    public Task UpdateVersion(string id, string version);
    public Task Delete(string id);
    public Task<IEnumerable<Project>> GetAll();
    public Task<Project> Get(string id);
}