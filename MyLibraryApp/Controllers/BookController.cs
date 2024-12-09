using Microsoft.AspNetCore.Mvc;
using MyLibraryApp.Services;
using MyLibraryApp.Shared;

namespace MyLibraryApp.Controllers;

[ApiController]
[Route("books")]
public class BookController : ControllerBase
{
    private IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Book book)
    {
        var existingBook = await _bookService.GetAsync(book.Id);

        if (existingBook is not null)
        {
            return Conflict();
        }

        await _bookService.AddAsync(book);

        return Ok();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var book = await _bookService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        await _bookService.DeleteAsync(id);

        return Ok();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Book>> Get(Guid id)
    {
        var book = await _bookService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        return Ok(book);
    }
        
    [HttpGet]
    public async Task<ActionResult<List<Book>>> Get()
    {
        return Ok(await _bookService.GetAllAsync());
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Book newBook)
    {
        if (id != newBook.Id)
        {
            return BadRequest();
        }

        var existingBook = await _bookService.GetAsync(id);

        if (existingBook is null)
        {
            return NotFound();
        }

        await _bookService.UpdateAsync(newBook);

        return Ok();
    }
    
}