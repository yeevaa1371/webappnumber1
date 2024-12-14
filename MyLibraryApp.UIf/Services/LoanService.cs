using System.Net.Http.Json;
using MyLibraryApp.Shared;

namespace MyLibraryApp.UIf.Services;

public class LoanService : ILoanService
{
    private readonly HttpClient _httpClient;

    public LoanService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<LoanWithDetails>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<LoanWithDetails>>("loan");
    }

    public Task AddAsync(Book book, Reader reader)
    {
        throw new NotImplementedException();
    }

    public Task<LoanWithDetails> GetAsync(Guid readerId)
    {
        throw new NotImplementedException();
    }
}