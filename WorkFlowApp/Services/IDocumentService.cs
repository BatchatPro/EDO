using WorkFlowApp.Models;

namespace WorkFlowApp.Services;

public interface IDocumentService
{
    Task<List<DocumentResponseModel>> GetAllDocuments();
}
