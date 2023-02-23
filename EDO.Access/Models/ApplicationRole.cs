using Microsoft.AspNetCore.Identity;

namespace EDO.Access.Models;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole() { }
    public ApplicationRole(string roleName, string roleTitle)
    {
        Name = roleName;
        Title = roleTitle;
    }
    public string Title { get; set; }
}