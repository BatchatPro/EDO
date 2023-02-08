using Microsoft.AspNetCore.Identity;
using System.Data.Common;

namespace EDO.Access;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole() { }
    public ApplicationRole(string roleName, string roleTitle)
    {
        this.Name = roleName;
        this.Title = roleTitle;
    }
    public string Title { get; set; }
}