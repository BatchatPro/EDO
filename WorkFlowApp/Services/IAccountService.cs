using WorkFlowApp.Models;

namespace WorkFlowApp.Services;

public interface IAccountService
{
    Task<bool> LoginAsync(LoginModel model);
    Task<bool> LogoutAsync();
}
