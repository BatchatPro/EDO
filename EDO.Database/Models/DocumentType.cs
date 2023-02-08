using NullGuard;

namespace EDO.Database.Models;

public class DocumentType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [AllowNull]
    public List<Document>? Documents { get; set; }
}
