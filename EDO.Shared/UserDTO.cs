using System.Diagnostics.CodeAnalysis;

namespace EDO.Shared;

public class UserDTO
{
    public string Id { get; set; }

    [AllowNull]
    public string? UserName { get; set; }

    [AllowNull]
    public string? FullName { get; set; }

    [AllowNull]
    public string? AttachedStatus { get; set; }
}
