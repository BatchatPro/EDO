using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

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
    public int DocumentTypeId { get; set; }
    [Required]
    public string FilePath { get; set; }
    [Required]
    public DateTime Deadline { get; set; }
    [Required]
    public string Status { get; set; }

    [AllowNull]
    public string? CreatedBy { get; set; }
    [AllowNull]
    public string? UpdatedBy { get; set; }
    [AllowNull]
    public DateTime? CreateDate { get; set; }
    [AllowNull]
    public DateTime? UpdateDate { get; set; }
    public List<UserDTO>? AttachedPeople { get; set; }
}
