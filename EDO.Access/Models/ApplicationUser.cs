using Microsoft.AspNetCore.Identity;
using NullGuard;

namespace EDO.Access.Models;

public class ApplicationUser : IdentityUser
{
    [AllowNull]
    public string? LastName { get; set; }
    [AllowNull]
    public string? FirstName { get; set; }
}