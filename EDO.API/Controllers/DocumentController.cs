﻿using EDO.Access.Models;
using EDO.Database;
using EDO.Database.Models;
using EDO.Database.Models.AccessReferences;
using EDO.Service.Mapper;
using EDO.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq;

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
                    DocumentTypeId = d.DocumentTypeId,
                    FilePath = d.FilePath,
                    Deadline = d.Deadline,
                    Status = d.Status,
                    CreatedBy = d.CreatedBy,
                    UpdatedBy = d.UpdatedBy,
                    CreateDate = d.CreateDate,
                    UpdateDate = d.UpdateDate,
                    AttachedPeople = d.DocumentUsers.Select(du => new UserDTO
                    {
                        Id = du.User.Id,
                        UserName = du.User.UserName,
                        FullName = $"{du.User.FirstName} {du.User.LastName}"
                    }).ToList()
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
                DocumentTypeId = d.DocumentTypeId,
                FilePath = d.FilePath,
                Deadline = d.Deadline,
                Status = d.Status,
                CreatedBy = d.CreatedBy,
                UpdatedBy = d.UpdatedBy,
                CreateDate = d.CreateDate,
                UpdateDate = d.UpdateDate,
                AttachedPeople = d.DocumentUsers.Select(du => new UserDTO
                {
                    Id = du.User.Id,
                    UserName = du.User.UserName,
                    FullName = $"{du.User.FirstName} {du.User.LastName}",
                    AttachedStatus = du.AttachedStatus
                }).ToList()
            }).ToList();

        return Ok(documents);
    }

    
    [HttpGet("{id}")]
    public async Task<ActionResult<DocumentDTO>> GetDocument(int id)
    {
        if (!User.IsInRole(RoleConst.ADMIN) && !await IsAuthor(id))
            return Forbid();

        //var document = await _context.Documents.FindAsync(id);
        var document = await _context.Documents
        .Include(d => d.DocumentType)
        .Include(d => d.DocumentUsers)
        .ThenInclude(du => du.User)
        .FirstOrDefaultAsync(d => d.Id == id);

        if (document == null)
            return NotFound();

        var attachedPeople = new List<UserDTO>();
        foreach (var documentUser in document.DocumentUsers)
            attachedPeople.Add(new UserDTO
            {
                Id = documentUser.User.Id,
                UserName = documentUser.User.UserName,
                FullName = $"{documentUser.User.FirstName} {documentUser.User.LastName}",
                AttachedStatus = documentUser.AttachedStatus
            });

        var documentDto = new DocumentDTO
        {
            Id = document.Id,
            Name = document.Name,
            Description = document.Description,
            DocumentTypeId = document.DocumentTypeId,
            FilePath = document.FilePath,
            Deadline = document.Deadline,
            Status = document.Status,
            CreatedBy = document.CreatedBy,
            UpdatedBy = document.UpdatedBy,
            CreateDate = document.CreateDate,
            UpdateDate = document.UpdateDate,
            AttachedPeople = attachedPeople
        };
        return Ok(documentDto);
    }


    [Route("AttachedDocuments")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DocumentDTO>>> GetAttachedUsers()
    {
        var userName = _userManager.GetUserId(User);
        var attachedUser = await _userManager.FindByNameAsync(userName);

        if (attachedUser == null)
            return BadRequest("Invalid user. User didn't fild in Users.");

        var documents = _context.Documents.Include(d => d.DocumentUsers).Where(d => d.DocumentUsers.Any(du => du.UserId == attachedUser.Id)).Select(d => new DocumentDTO
        {
            Id = d.Id,
            Name = d.Name,
            Description = d.Description,
            DocumentTypeId = d.DocumentTypeId,
            FilePath = d.FilePath,
            Deadline = d.Deadline,
            Status = d.Status,
            CreatedBy = d.CreatedBy,
            UpdatedBy = d.UpdatedBy,
            CreateDate = d.CreateDate,
            UpdateDate = d.UpdateDate,
            AttachedPeople = d.DocumentUsers.Select(du => new UserDTO
            {
                Id = du.User.Id,
                UserName = du.User.UserName,
                FullName = $"{du.User.FirstName} {du.User.LastName}",
                AttachedStatus = du.AttachedStatus
            }).ToList()
        }).ToList();
        
        return (documents == null) ? NotFound():  Ok(documents);
    }


    [HttpPut]
    public async Task<IActionResult> UpdateDocument(DocumentDTO documentDTO)
    {
        var userName = _userManager.GetUserId(User);
        var updateBy = await _userManager.FindByNameAsync(userName);

        if (updateBy == null)
            return BadRequest("Invalid user. User didn't fild in Users.");

        var document = await _context.Documents.Include(d => d.DocumentUsers)
            .ThenInclude(du => du.User)
            .SingleOrDefaultAsync(d => d.Id == documentDTO.Id);

        if (document == null)
            return NotFound();

        document.Name = documentDTO.Name;
        document.Description = documentDTO.Description;
        document.DocumentTypeId = documentDTO.DocumentTypeId;
        document.FilePath = documentDTO.FilePath;
        document.Deadline = documentDTO.Deadline;
        document.Status = documentDTO.Status;
        document.UpdatedBy = updateBy.Id;
        document.UpdateDate = DateTime.Now;

        // update attached people
        var attachedPeople = new List<UserDTO>();
        if (documentDTO.AttachedPeople != null)
        {
            foreach (var userDTO in documentDTO.AttachedPeople)
            {
                var user = await _context.ApplicationUserReferences.FindAsync(userDTO.Id);
                if (user != null)
                {
                    attachedPeople.Add(new UserDTO
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        FullName = user.FirstName + " " + user.LastName,
                    });

                    // Add new DocumentUser relationship if not already exists
                    var existingRelationship = document.DocumentUsers.SingleOrDefault(du => du.UserId == user.Id);
                    if (existingRelationship == null)
                    {
                        var newRelationship = new DocumentUser
                        {
                            User = user,
                            CreatedBy = updateBy.UserName,
                            CreateDate = DateTime.Now,
                            Document = document
                        };
                        document.DocumentUsers.Add(newRelationship);
                    }
                }
            }

            var removedUsers = document.DocumentUsers.Where(du => !attachedPeople.Any(ap => ap.Id == du.UserId)).ToList();
            foreach (var removedUser in removedUsers)
            {
                document.DocumentUsers.Remove(removedUser);

                _context.DocumentUsers.Remove(removedUser);
            }

        }
        await _context.SaveChangesAsync();

        return NoContent();
    }


    [Route("ChackDocument")]
    [HttpPut]
    public async Task<IActionResult> ChackDocumeUser(DocumentUserChackDTO chackDocumeUser)
    {
        var userName = _userManager.GetUserId(User);
        var user = await _userManager.FindByNameAsync(userName);


        if (user == null)
            return BadRequest("Invalid user. User didn't fild in Users.");

        var chackDocument = _context.DocumentUsers.Where(d => d.UserId == user.Id && d.DocumentId == chackDocumeUser.DocumentId).FirstOrDefault();

        if (chackDocument == null)
            return NotFound();

        chackDocument.AttachedStatus = chackDocumeUser.AttachedStatus;
        await _context.SaveChangesAsync();

        return Ok(chackDocumeUser);
    }

    [HttpPost]
    public async Task<ActionResult<DocumentDTO>> PostDocument(DocumentDTO documentDTO)
    {
        var userName = _userManager.GetUserId(User);
        var createdBy = await _userManager.FindByNameAsync(userName);
        //var createdBy = await _userManager.GetUserAsync(HttpContext.User);
        if (createdBy == null)
            return BadRequest("Invalid user. User didn't fild in Users.");

        var attachedPeople = new List<UserDTO>();
        if (documentDTO.AttachedPeople != null)
        {
            foreach (var userDTO in documentDTO.AttachedPeople)
            {
                var user = await _context.ApplicationUserReferences.FindAsync(userDTO.Id);
                if (user != null)
                attachedPeople.Add(new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FirstName+" "+ user.LastName
                });
            }
        }

        var document = new Document
        {
            Name = documentDTO.Name,
            Description = documentDTO.Description,
            DocumentTypeId = documentDTO.DocumentTypeId,
            FilePath = documentDTO.FilePath,
            Deadline = documentDTO.Deadline,
            Status = documentDTO.Status,
            CreatedBy = createdBy.Id,
            CreateDate = DateTime.Now,
            DocumentUsers = new List<DocumentUser>()
        };


        foreach (UserDTO userDTO in attachedPeople)
        {
            var user =  await _context.ApplicationUserReferences.FindAsync(userDTO.Id);
            var documentUser = new DocumentUser
            {
                User = user,
                CreatedBy = createdBy.Id,
                CreateDate = DateTime.Now,
                Document = document
            };
            document.DocumentUsers.Add(documentUser);
        }

        _context.Documents.Add(document);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, new DocumentDTO
        {
            Id = document.Id,
            Name = document.Name,
            Description = document.Description,
            FilePath = document.FilePath,
            Status = document.Status,
            CreatedBy = document.CreatedBy,
            DocumentTypeId = document.DocumentTypeId,
            CreateDate = document.CreateDate,
            Deadline = document.Deadline,
            AttachedPeople = attachedPeople.Select(u => new UserDTO
            {
                Id = u.Id,
                UserName = u.UserName,
                FullName = u.FullName
            }).ToList()
        });
    }


    [Authorize(Roles = RoleConst.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDocument(int id)
    {

        var document = await _context.Documents.FindAsync(id);

        if (document == null)
        {
            return NotFound();
        }

        // Remove associated DocumentUser records
        var documentUsers = _context.DocumentUsers.Where(du => du.DocumentId == id);
        _context.DocumentUsers.RemoveRange(documentUsers);

        // Remove the document
        _context.Documents.Remove(document);

        await _context.SaveChangesAsync();

        return NoContent();
    }


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