using Newtonsoft.Json;
using System.Text;
using WorkFlowApp.Models;

namespace WorkFlowApp.Services;

public class LoginService : ILoginService
{
    private readonly HttpClient _httpClient;

    public LoginService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> LoginAsync(LoginRequest loginRequest)
    {
        var jsonRequest = JsonConvert.SerializeObject(loginRequest);
        var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://localhost:44369/login", stringContent);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        return jsonResponse;
    }
}
