using WorkFlowApp.Models;

namespace WorkFlowApp.Services;

public class AccountService : IAccountService
{
    public Task<bool> LoginAsync(LoginModel model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> LogoutAsync()
    {
        throw new NotImplementedException();
    }
}
