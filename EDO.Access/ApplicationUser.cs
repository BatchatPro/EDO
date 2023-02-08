using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace EDO.Access;

public class ApplicationUser : IdentityUser
{
    [AllowNull]
    public string? LastName { get; set; }
    [AllowNull]
    public string? FirstName { get; set; }
}