namespace WorkFlowApp.Models;

public class LoginResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public string UserKey { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string UserFullName { get; set; }
    public string[] Access { get; set; }
}
