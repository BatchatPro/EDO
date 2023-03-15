using System.ComponentModel.DataAnnotations;

namespace EDO.Shared;

public class DocumentDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string FilePath { get; set; }
    [Required]
    public string Status { get; set; }
    [Required]
    public string AuthorUserName { get; set; }
    [Required]
    public int DocumentTypeId { get; set; }
}
