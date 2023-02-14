using EDO.WorkFlow.Models;

namespace EDO.WorkFlow.Services;

public interface IAccountService
{
    Task<bool> LoginAsync(LoginModel model);
    Task<bool> LogoutAsync();
}
