using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WorkFlowApp.Models;

namespace WorkFlowApp.Services;

public class LoginService : ILoginService
{
    HttpClient _httpClient;
    public LoginService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    //private string _baseUrl = "https://localhost:44369";

    public async Task<LoginResponseModel> Login(string userName, string password)
    {
        _httpClient.BaseAddress = new Uri("https://localhost:44369/");
        //var myContent = JsonConvert.SerializeObject(new
        //{
        //    userName,
        //    password
        //});
        //var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        //var byteContent = new ByteArrayContent(buffer);
        //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await _httpClient.PostAsJsonAsync("login", new
        {
            userName,
            password
        });

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            LoginResponseModel loginResponseModel = JsonConvert.DeserializeObject<LoginResponseModel>(content);
            Debug.WriteLine(loginResponseModel.Token);
            return loginResponseModel;
        }

        else
            return null;
    }
}
