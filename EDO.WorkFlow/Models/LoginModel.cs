using System.ComponentModel.DataAnnotations;

namespace EDO.WorkFlow.Models;

public class LoginModel
{
    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public bool LoginFailureHidden { get; set; } = true;

    // Add Role
    public int RoleID { get; set; }
    public string RoleName { get; set; }
}

public enum RoleDetails
{
    Student = 1,
    Teacher,
    Admin,
}
