using EDO.Database;
using EDO.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDO.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DocumentsController : ControllerBase
{
    private readonly EdoDbContext _context;

    public DocumentsController(EdoDbContext context)
    {
        _context = context;
    }

    // GET: api/Documents
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Document>>> GetDocuments()
    {
        return (_context.Documents == null) ? NotFound(): await _context.Documents.ToListAsync();
    }

    // GET: api/Documents/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Document>> GetDocument(int id)
    {
        var document = await _context.Documents.FindAsync(id);

        return (document == null) ? NotFound() : document;
    }

    // PUT: api/Documents
    [HttpPut]
    public async Task<IActionResult> PutDocument(Document document)
    {
        if(!DocumentExists(document.Id))
            return NotFound($"Not Found element with this id: {document.Id}");
        
        _context.Documents.Update(document);
        await _context.SaveChangesAsync();
        
        return Ok(document);
    }

    // POST: api/Documents
    [HttpPost]
    public async Task<ActionResult<Document>> PostDocument(Document document)
    {
        _context.Documents.Add(document);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, document);
    }

    // DELETE: api/Documents/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Document>> DeleteDocument(int id)
    {
        var document = await _context.Documents.FindAsync(id);
        if (document == null)
        {
            return NotFound($"Not Found Element with this id: {id}");
        }

        _context.Documents.Remove(document);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool DocumentExists(int id)
    {
        return _context.Documents.Any(e => e.Id == id);
    }
}