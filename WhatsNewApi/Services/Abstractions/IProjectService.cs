using WhatsNewApi.Models.FirestoreModels;

namespace WhatsNewApi.Services.Abstractions
{
    public interface IProjectService
    {
        public Task CreateProject(string projectName, string projectVersion);
        public Task UpdateVersion(string id, string version);
        public Task DeleteProject(string id);
        public Task<IEnumerable<Project>> GetProjects();
        public Task<Project> GetProject(string id);
        public Task AddWhatsNew(string id, string version, IEnumerable<WhatsNewPage> pages);
        public Task<WhatsNew> GetWhatsNew(string id, string version);
        public Task UpdateWhatsNew(string id, string version, WhatsNew whatsNew);
        public Task DeleteWhatsNew(string id, string version);
    }
}