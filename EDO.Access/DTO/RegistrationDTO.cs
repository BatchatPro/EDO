using NullGuard;
using System.ComponentModel.DataAnnotations;

namespace EDO.Access.DTO;

public class RegistrationDTO
{
    [Required]
    public string UserName { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "Minimum Length = 6 !", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "It is not the same Password!")]
    public string ConfirmPassword { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    [AllowNull]
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}
