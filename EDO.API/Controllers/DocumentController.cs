using EDO.Database;
using EDO.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDO.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DocumentController : Controller
{
    private readonly EdoDbContext _dbContext;

    public DocumentController(EdoDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    // GET: api/Document/GetAll
    [HttpGet]
    [Route("Document/GetDocuments")]
    public async Task<ActionResult<IEnumerable<Document>>> GetDocuments()
    {
        if (_dbContext.Documents == null)
        {
            return NotFound();
        }
        return await _dbContext.Documents.ToListAsync();
    }


    // GET: api/Document/GetAll/5
    [HttpGet]
    [Route("Document/GetDocument{id:int}")]
    public async Task<ActionResult<Document>> GetDocument(int id)
    {
        if (_dbContext.Documents == null)
        {
            return NotFound();
        }
        var document = await _dbContext.Documents.FindAsync(id);

        if (document == null)
        {
            return NotFound();
        }

        return document;
    }


    // POST: api/Documents/GetAll/1
    [HttpPost]
    [Route("Document/PostMovie")]
    public async Task<ActionResult<Document>> PostMovie(Document document)
    {
        _dbContext.Documents.Add(document);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDocuments), new { id = document.Id }, document);
    }


    // PUT: api/Document/GetAll/4
    [HttpPut]
    [Route("Document/PutDocument{id:int}")]
    public async Task<IActionResult> PutDocument(int id, Document document)
    {
        if (id != document.Id)
        {
            return BadRequest();
        }

        _dbContext.Entry(document).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DocumentExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }


    // DELETE: api/Movies/5
    [HttpDelete]
    [Route("Document/DeleteDocument{id:int}")]
    public async Task<IActionResult> DeleteDocument(int id)
    {
        if (_dbContext.Documents == null)
        {
            return NotFound();
        }

        var movie = await _dbContext.Documents.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        _dbContext.Documents.Remove(movie);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool DocumentExists(long id)
    {
        return (_dbContext.Documents?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
