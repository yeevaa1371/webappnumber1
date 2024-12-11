using System.Net.Http.Json;
using MyLibraryApp.Shared;

namespace MyLibraryApp.UI.Services;

public class BookService : IBookService
{
    private readonly HttpClient _httpClient;

    public BookService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<List<Book>> GetBooksAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Book>>("api/books");
    }

    public async Task<Book> GetBookByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<Book>($"api/books/{id}");
    }

    public async Task AddBookAsync(Book book)
    {
        await _httpClient.PostAsJsonAsync("api/books", book);
    }

    public async Task UpdateBookAsync(Book book)
    {
        await _httpClient.PutAsJsonAsync($"api/books/{book.Id}", book);
    }

    public async Task DeleteBookAsync(Guid id)
    {
        await _httpClient.DeleteAsync($"api/books/{id}");
    }
}