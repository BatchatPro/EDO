using WorkFlowApp.Models;

namespace WorkFlowApp.Services;

public interface ILoginService
{
    Task<string> LoginAsync(LoginRequest loginRequest);
}
