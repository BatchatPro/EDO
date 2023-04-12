using EDO.Access.Models;
using EDO.Database;
using EDO.Service.Mapper;
using EDO.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDO.API.Controllers;

[Authorize(Roles = RoleConst.STUFF)]
[Route("api/[controller]")]
[ApiController]
public class DocumentsController : ControllerBase
{
    private readonly EdoDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DocumentsController(EdoDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        this._userManager = userManager;
    }

    // GET: api/Documents
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DocumentDTO>>> GetDocuments()
    {
        if (User.IsInRole(RoleConst.ADMIN))
            return (_context.Documents == null) ? NotFound() : (_context.Documents
                .Include(d => d.DocumentUsers)
                .ThenInclude(du => du.User)
                .Select(d => new DocumentDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    FilePath = d.FilePath,
                    Status = d.Status,
                    CreatedBy = d.CreatedBy,
                    DocumentTypeId = d.DocumentTypeId,
                    CreateDate = d.CreateDate,
                    Deadline = d.Deadline,
                    AttachedPeople = d.DocumentUsers.Select(du => du.User.UserName).ToList()
                }).ToList());

        var userName = _userManager.GetUserId(User);
        var user = await _userManager.FindByNameAsync(userName);

        var documents = _context.Documents
            .Where(d => d.CreatedBy == user.Id)
            .Include(d => d.DocumentUsers)
            .ThenInclude(du => du.User)
            .Select(d => new DocumentDTO
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                FilePath = d.FilePath,
                Status = d.Status,
                CreatedBy = d.CreatedBy,
                DocumentTypeId = d.DocumentTypeId,
                CreateDate = d.CreateDate,
                Deadline = d.Deadline,
                AttachedPeople = d.DocumentUsers.Select(du => du.User.UserName).ToList()
            }).ToList();

        return Ok(documents);
    }

    //// GET: api/Documents
    //[HttpGet]
    //public async Task<ActionResult<IEnumerable<DocumentDTO>>> GetDocuments()
    //{
    //    if (User.IsInRole(RoleConst.ADMIN))
    //        return (_context.Documents == null) ? NotFound() : (await _context.Documents.Select(x => new DocumentDTO
    //        {
    //            Id = x.Id,
    //            Name = x.Name,
    //            Description = x.Description,
    //            DocumentTypeId = x.DocumentTypeId,
    //            FilePath = x.FilePath,
    //            CreatedBy = x.CreatedBy,
    //            Deadline = x.Deadline,
    //            CreateDate = x.CreateDate,
    //            Status = x.Status,
    //        }).ToListAsync());

    //    var userName = _userManager.GetUserId(User);
    //    var user = await _userManager.FindByNameAsync(userName);

    //    var documents = await _context.Documents
    //    .Where(d => d.CreatedBy == user.Id)
    //    .Select(x => new DocumentDTO
    //    {
    //        Id = x.Id,
    //        Name = x.Name,
    //        Description = x.Description,
    //        DocumentTypeId = x.DocumentTypeId,
    //        FilePath = x.FilePath,
    //        CreatedBy = x.CreatedBy,
    //        Deadline = x.Deadline,
    //        CreateDate = x.CreateDate,
    //        Status = x.Status
    //    })
    //    .ToListAsync();

    //    return Ok(documents);
    //}


    //// GET: api/Documents/5
    //[HttpGet("{id}")]
    //public async Task<ActionResult<DocumentDTO>> GetDocument(int id)
    //{
    //    if (!User.IsInRole(RoleConst.ADMIN) && !await IsAuthor(id))
    //        return Forbid();

    //    var document = await _context.Documents.FindAsync(id);

    //    return (document == null) ? NotFound($"Not Found element with this id: {id}") : document.ConvertToDTO();
    //}

    //// PUT: api/Documents
    //[HttpPut]
    //public async Task<IActionResult> PutDocument(DocumentDTO document)
    //{
    //    if (!DocumentExists(document.Id))
    //        return NotFound($"Not Found element with this id: {document.Id}");

    //    if (!User.IsInRole(RoleConst.ADMIN) && !await IsAuthor(document.Id))
    //        return Forbid();

    //    var userName = _userManager.GetUserId(User);
    //    var user = await _userManager.FindByNameAsync(userName);

    //    document.CreatedBy = user.Id;

    //    _context.Documents.Update(document.ConvertToEntity());
    //    await _context.SaveChangesAsync();

    //    return Ok(document);
    //}

    //// POST: api/Documents
    //[HttpPost]
    //public async Task<ActionResult<DocumentDTO>> PostDocument(DocumentDTO document)
    //{
    //    var userName = _userManager.GetUserId(User);
    //    var user = await _userManager.FindByNameAsync(userName);

    //    document.CreatedBy = user.Id;
    //    _context.Documents.Add(document.ConvertToEntity());
    //    await _context.SaveChangesAsync();

    //    return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, document);
    //}

    //// DELETE: api/Documents/5
    //[HttpDelete("{id}")]
    //public async Task<ActionResult<DocumentDTO>> DeleteDocument(int id)
    //{
    //    if (!User.IsInRole(RoleConst.ADMIN) && !await IsAuthor(id))
    //        return Forbid();

    //    var document = await _context.Documents.FindAsync(id);
    //    if (document == null)
    //        return NotFound($"Not Found Element with this id: {id}");

    //    _context.Documents.Remove(document);
    //    await _context.SaveChangesAsync();

    //    return NotFound();
    //}

    private bool DocumentExists(int id)
    {
        return _context.Documents.Any(e => e.Id == id);
    }

    private async Task<bool> IsAuthor(int documentId)
    {
        var document = await _context.Documents.FindAsync(documentId);
        if (document == null)
            return false;

        var userName = _userManager.GetUserId(User);
        var user = await _userManager.FindByNameAsync(userName);
        if (user.Id == null)
            return false;
        
        return user.Id == document.CreatedBy;
    }
}