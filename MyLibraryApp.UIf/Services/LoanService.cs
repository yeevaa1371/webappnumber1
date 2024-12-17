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

    public async Task<List<LoanWithDetails>> GetByReaderAsync(Guid readerId)
    {
        return await _httpClient.GetFromJsonAsync<List<LoanWithDetails>>($"/loan/reader/{readerId}");
    }

    public async Task<List<LoanWithDetails>> GetByBookAsync(Guid bookId)
    {
        return await _httpClient.GetFromJsonAsync<List<LoanWithDetails>>($"/loan/book/{bookId}");
    }

}