using EDO.WorkFlow.Models;
using Newtonsoft.Json;

namespace EDO.WorkFlow.Services;

public class DocumentService: IDocumentService
{
    private string _baseUrl = "https://localhost:44369";
    public async Task<List<DocumentResponseModel>> GetAllDocuments()
    {
        var returnResponse = new List<DocumentResponseModel>();
        using (var client = new HttpClient())
        {
            string url = $"{_baseUrl}/api/Documents";
            var apiResponse = await client.GetAsync(url);

            if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var response =await apiResponse.Content.ReadAsStringAsync();
                returnResponse = JsonConvert.DeserializeObject<List<DocumentResponseModel>>(response);
            }
        }
        return returnResponse;
    }
}
