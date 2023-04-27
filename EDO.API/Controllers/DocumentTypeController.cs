using EDO.Database;
using EDO.Database.Models;
using EDO.Service.Mapper;
using EDO.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDO.API.Controllers;

[Authorize(Roles = RoleConst.STUFF)]
[Route("api/[controller]")]
[ApiController]
public class DocumentTypeController : ControllerBase
{
    private readonly EdoDbContext _context;

    public DocumentTypeController(EdoDbContext context)
    {
        _context = context;
    }

    // GET: api/DocumentTypes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DocumentTypeDTO>>> GetDocumentTypes()
    {
        return (_context.DocumentTypes == null) ? NotFound() : await _context.DocumentTypes.Select(x => new DocumentTypeDTO()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description
        }).ToListAsync();
    }

    // GET: api/DocumentTypes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DocumentTypeDTO>> GetDocumentType(int id)
    {
        var documentType = await _context.DocumentTypes.FindAsync(id);

        return (documentType == null) ? NotFound() : documentType.ConvertToDTO();
    }

    // PUT: api/DocumentTypes
    [Authorize(Roles = RoleConst.MAIN)]
    [HttpPut]
    public async Task<IActionResult> PutDocumentType(DocumentTypeDTO documentType)
    {
        if (!DocumentTypeExists(documentType.Id))
            return NotFound($"Not Found element with this id: {documentType.Id}");

        _context.DocumentTypes.Update(documentType.ConvetToEntity());
        await _context.SaveChangesAsync();

        return Ok(documentType);
    }

    // POST: api/DocumentTypes
    [Authorize(Roles = RoleConst.MAIN)]
    [HttpPost]
    public async Task<ActionResult<DocumentType>> PostDocumentType(DocumentTypeDTO documentType)
    {
        _context.DocumentTypes.Add(documentType.ConvetToEntity());
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDocumentType), new { id = documentType.Id }, documentType);
    }

    // DELETE: api/DocumentTypes/5
    [Authorize(Roles = RoleConst.MAIN)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<DocumentTypeDTO>> DeleteDocumentType(int id)
    {
        var documentType = await _context.DocumentTypes.FindAsync(id);
        if (documentType == null)
        {
            return NotFound($"Not Found Element with this id: {id}");
        }

        _context.DocumentTypes.Remove(documentType);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool DocumentTypeExists(int id)
    {
        return _context.DocumentTypes.Any(e => e.Id == id);
    }
}
