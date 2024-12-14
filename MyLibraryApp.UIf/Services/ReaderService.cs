using System.Net.Http.Json;
using MyLibraryApp.Shared;

namespace MyLibraryApp.UIf.Services;

public class ReaderService : IReaderService
{
    private readonly HttpClient _httpClient;

    public ReaderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Reader>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Reader>>("readers");
    }

    public async Task AddAsync(Reader reader)
    {
        await _httpClient.PostAsJsonAsync("reders", reader);
    }

    public async Task<Reader> GetAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<Reader>($"readers/{id}");
    }

    public async Task DeleteAsync(Guid id)
    {
        await _httpClient.DeleteAsync($"readers/{id}");
    }

    public async Task UpdateAsync(Reader reader)
    {
        await _httpClient.PutAsJsonAsync<Reader>($"readers/{reader.Id}", reader);
    }
}