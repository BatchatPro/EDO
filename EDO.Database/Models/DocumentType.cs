using EDO.Domain;
using NullGuard;

namespace EDO.Database.Models;

public class DocumentType:BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    [AllowNull]
    public List<Document>? Documents { get; set; }
}
