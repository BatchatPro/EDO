using NullGuard;
using System.ComponentModel.DataAnnotations;

namespace EDO.Access.DTO;

public class RegistrationDTO
{
    [Required(ErrorMessage = "This Poly is Required.")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "This Poly is Required.")]
    [StringLength(100, ErrorMessage = "Minimum Length = 6 !", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required(ErrorMessage = "This Poly is Required.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "It is not the same Password!")]
    public string ConfirmPassword { get; set; }
    [Required(ErrorMessage = "This Poly is Required.")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "This Poly is Required.")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "This Poly is Required.")]
    public string ThirdName { get; set; }
    [AllowNull]
    public string? Email { get; set; }
    [Required(ErrorMessage = "This Poly is Required.")]
    public string PhoneNumber { get; set; }
}
