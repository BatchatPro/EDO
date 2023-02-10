using EDO.WorkFlow.Models;

namespace EDO.WorkFlow.Services;

public interface IDocumentService
{
    Task<List<DocumentResponseModel>> GetAllDocuments();
}
