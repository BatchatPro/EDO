using System.ComponentModel.DataAnnotations;

namespace EDO.Shared;

public class DocumentTypeDTO
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
}
