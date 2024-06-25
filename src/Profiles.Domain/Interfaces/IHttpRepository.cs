namespace Profiles.Domain.Interfaces;

public interface IHttpRepository<T> where T : class
{
    public Task<List<T>> GetCollection(IEnumerable<string> officesIds);

    public Task<T?> GetOneAsync(string url, string id);
}
