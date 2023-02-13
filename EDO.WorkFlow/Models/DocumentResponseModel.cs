using NullGuard;

namespace EDO.WorkFlow.Models;

public class DocumentResponseModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string FilePath { get; set; }
    public int DocumentTypeId { get; set; }
    [AllowNull]
    public string[]? DocumentType { get; set; }
}

