using WhatsNewApi.Extensions;
using WhatsNewApi.Models.Exceptions;
using WhatsNewApi.Models.FirestoreModels;
using WhatsNewApi.Repos.Abstractions;
using WhatsNewApi.Services.Abstractions;

namespace WhatsNewApi.Services;
public class ProjectService : IProjectService
{
    private readonly ILogger<ProjectService> _logger;
    private readonly IProjectRepository _repo;

    public ProjectService(ILogger<ProjectService> logger, IProjectRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task CreateProject(string projectName, string projectVersion)
    {
        try
        {
            await _repo.Create(new Project
            {
                CurrentVersion = projectVersion,
                Name = projectName
            });
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            throw new FirebaseException($"Creating a project has failed with" +
                $" the following: {ex.Message}");
        }
    }

    public async Task DeleteProject(string id)
    {
        try
        {
            await _repo.Delete(id);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            throw new FirebaseException($"Deleting a project has failed with" +
                $" the following: {ex.Message}");
        }
    }

    public async Task<Project> GetProject(string id)
    {
        try
        {
            return await _repo.Get(id);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            throw new FirebaseException($"Fetching project with id {id}" +
                $" has failed with the following: {ex.Message}");
        }
    }

    public async Task<IEnumerable<Project>> GetProjects()
    {
        try
        {
            return await _repo.GetAll();
        }
        catch(Exception ex)
        {
            _logger.LogException(ex);
            throw new FirebaseException($"Fetching all projects has failed" +
                $" with the following: {ex.Message}");

        }
    }

    public async Task UpdateVersion(string id, string version)
    {
        try
        {
            var project = await _repo.Get(id);
            project.CurrentVersion = version;
            await _repo.Update(project);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            throw new FirebaseException($"Adding a whats new to project id " +
                $"{id} has failed with the following: {ex.Message}");
        }
    }

    public async Task AddWhatsNew(string id, string version, IEnumerable<WhatsNewPage> pages)
    {
        try
        {
            var project = await _repo.Get(id);
            var whatsnew = new WhatsNew
            {
                Version = version,
                Pages = pages.ToList()
            };
            project.WhatsNews.Add(whatsnew);
            await _repo.Update(project);

        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            throw new FirebaseException($"Adding a whats new to project id" +
                $" {id} has failed with the following: {ex.Message}");
        }
    }

    public async Task<WhatsNew> GetWhatsNew(string id, string version)
    {
        try
        {
            var project = await _repo.Get(id);
            var whatsNew = project.WhatsNews.First(wn => wn.Version.Equals(version));
            if (whatsNew != null)
                return whatsNew;
            throw new ArgumentException($"Project with id: {id} has no " +
                $"version {version}");
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            throw new FirebaseException("Fetching a WhatsNew has failed with" +
                " the following: Project with Id({id}) doesn't exist");
        }
    }
}
