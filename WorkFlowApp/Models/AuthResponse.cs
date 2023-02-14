namespace WorkFlowApp.Models;

public class AuthResponse
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public string UserFullName { get; set; }
    public string PINFL { get; set; }
    //public string RefreshToken { get; set; }
    //[AllowNull]
    //public List<string> Access { get; set; }
}
