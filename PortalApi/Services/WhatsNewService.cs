using System;
using WhatsNewApi.Extensions;
using WhatsNewApi.Models.Exceptions;
using WhatsNewApi.Models.FirestoreModels;
using WhatsNewApi.Repos.Abstractions;
using WhatsNewApi.Services.Abstractions;

namespace WhatsNewApi.Services
{
	public class WhatsNewService : IWhatsNewService
	{
        private readonly ILogger<ProjectService> _logger;
        private readonly IFirestoreRepository<WhatsNew> _repo;

        public WhatsNewService(ILogger<ProjectService> logger, IFirestoreRepository<WhatsNew> repo)
		{
            _logger = logger;
            _repo = repo;
        }

        public async Task<IEnumerable<WhatsNew>> GetAllWhatsNews(string projectId)
        {
            try
            {
                var whatsNews = await _repo.GetAll();
                if(whatsNews.Any())
                    return whatsNews.Where(wn => wn.ProjectId == projectId);
                throw new ArgumentException($"Project with id: {projectId} has no Whats News");
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw new FirebaseException($"Fetching all WhatsNews for project {projectId}" +
                    $" has failed with the following: {ex.Message}");
            }
        }

        public async Task<WhatsNew> GetWhatsNew(string projectId, string version)
        {
            try
            {
                var whatsNews = await _repo.GetAll();
                var whatsNew = whatsNews.Where(wn =>
                    wn.ProjectId == projectId && wn.Version == version).First();

                if (whatsNew != null)
                    return whatsNew;

                throw new ArgumentException($"Project with id: {projectId} has no " +
                    $"version {version}");
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw new FirebaseException($"Fetching a WhatsNew has failed with" +
                    $" the following: {ex.Message}");
            }
        }

        public async Task CreateWhatsNew(string id, string version, IEnumerable<WhatsNewPage> pages)
        {
            try
            {
                var whatsnew = new WhatsNew
                {
                    ProjectId = id,
                    Version = version,
                    Pages = pages.ToList()
                };
                await _repo.Create(whatsnew);

            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw new FirebaseException($"Adding a whats new to project id" +
                    $" {id} has failed with the following: {ex.Message}");
            }
        }

        public async Task UpdateWhatsNew(string id, WhatsNew wn)
        {
            try
            {
                await _repo.Update(id, wn);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw new FirebaseException($"Updating a WhatsNew has failed with" +
                    $" the following: {ex.Message}");
            }
        }

        public async Task DeleteWhatsNew(string id)
        {
            try
            {
                await _repo.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw new FirebaseException($"Deleting a Whats New failed with the" +
                    $" following: {ex.Message}");
            }
        }
    }
}

