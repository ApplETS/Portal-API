using System;
using WhatsNewApi.Models.FirestoreModels;

namespace WhatsNewApi.Services.Abstractions
{
	public interface IWhatsNewService
    {
        public Task<IEnumerable<WhatsNew>> GetAllWhatsNews(string projectId);
        public Task<WhatsNew> GetWhatsNew(string projectId, string version);
        public Task<IEnumerable<WhatsNew>> GetWhatsNewsInRange(string projectId, string from, string to);
        public Task CreateWhatsNew(string projectId, string version, IEnumerable<WhatsNewPage> pages);
        public Task UpdateWhatsNew(string id, WhatsNew whatsNew);
        public Task DeleteWhatsNew(string id);
    }
}

