using System.ComponentModel.DataAnnotations;

namespace EDO.Shared;

public class CreateDocumentDTO
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string FilePath { get; set; }
    [Required]
    public string Status { get; set; }
    public string CreatedBy { get; set; }
    [Required]
    public int DocumentTypeId { get; set; }
    public DateTime CreateDate { get; set; }
    [Required]
    public DateTime Deadline { get; set; }
    public List<UserDTO>? AttachedPeople { get; set; }
}
