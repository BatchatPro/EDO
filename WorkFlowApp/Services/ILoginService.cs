using WorkFlowApp.Models;

namespace WorkFlowApp.Services;

public interface ILoginService
{
    Task<LoginResponseModel> Login(string userName, string password);
}
