using WhatsNewApi.Models.FirestoreModels;

namespace WhatsNewApi.Repos.Abstractions;

public interface IProjectRepository
{
	public Task Create(Project project);
    public Task Update(Project project);
    public Task Delete(string id);
    public Task<IEnumerable<Project>> GetAll();
    public Task<Project> Get(string id);
}