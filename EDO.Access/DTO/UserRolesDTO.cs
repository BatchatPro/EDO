using NullGuard;
namespace EDO.Access.DTO;

public class UserRolesDTO
{
    public string? UserId { get; set; }
    [AllowNull]
    public IEnumerable<RoleDTO>? Roles { get; set; }
}