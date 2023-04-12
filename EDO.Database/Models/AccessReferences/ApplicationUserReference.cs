using Microsoft.AspNetCore.Identity;
using NullGuard;

namespace EDO.Database.Models.AccessReferences;

public class ApplicationUserReference: IdentityUser
{
    [AllowNull]
    public string? LastName { get; set; }
    [AllowNull]
    public string? FirstName { get; set; }

    public List<DocumentUser> DocumentUsers { get; set; } = new();

}
