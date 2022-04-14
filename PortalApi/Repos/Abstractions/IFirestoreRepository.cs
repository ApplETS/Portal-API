using WhatsNewApi.Models.FirestoreModels;

namespace WhatsNewApi.Repos.Abstractions;

public interface IFirestoreRepository<T>
{
	public Task Create(T document);
    public Task Update(string id, T documentToUpdate);
    public Task Delete(string id);
    public Task<IEnumerable<T>> GetAll();
    public Task<T> Get(string id);
}