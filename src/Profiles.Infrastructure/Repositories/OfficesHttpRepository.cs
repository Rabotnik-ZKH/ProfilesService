using Profiles.Domain.Interfaces;
using System.Net.Http.Json;

namespace Profiles.Infrastructure.Repositories;

public class OfficesHttpRepository<T> : IHttpRepository<T> where T : class
{
    private readonly HttpClient _httpClient;

    public OfficesHttpRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<T>> GetCollection(IEnumerable<string> officesIds)
    {
        string stringWithOfficesIds = string.Join(',', officesIds);

        return await _httpClient.GetFromJsonAsync<List<T>>($"https://localhost:7255/api/offices/collection/({stringWithOfficesIds})");
    }

    public async Task<T?> GetOneAsync(string url, string id)
    {
        return await _httpClient.GetFromJsonAsync<T>($"{url}/{id}");
    }
}
