using EDO.Domain;
using NullGuard;

namespace EDO.Database.Models;

public class Document:BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string FilePath { get; set; }
    public string Status { get; set; }
    public DateTime Deadline { get; set; } = DateTime.Now + new TimeSpan(5, 0, 0, 0);
    public int DocumentTypeId { get; set; }
    [AllowNull]
    public DocumentType? DocumentType { get; set; }
}
