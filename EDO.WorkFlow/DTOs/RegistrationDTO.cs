using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EDO.WorkFlow.DTOs;

public class RegistrationDTO
{
    [Required]
    public string UserName { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "Пароль должен содержать не менее 6 символов!", MinimumLength = 6)]
    [DataType(DataType.Password)]

    public string Password { get; set; }
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
    public string ConfirmPassword { get; set; }
    [Required(ErrorMessage = "Это поле обязательно к заполнению!")]
    [StringLength(50, ErrorMessage = "Фамилия превышает 50 символов!")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Это поле обязательно к заполнению!")]
    [StringLength(50, ErrorMessage = "Имя превышает 50 символов!")]
    public string FirstName { get; set; }
    [AllowNull]
    public string MiddelName { get; set; }
    [AllowNull]
    public string Email { get; set; }

    [Required]
    [RegularExpression(@"^[0][1-9]([.][0-9][0-9]){4}", ErrorMessage = "Incorrect phone number !")]
    public string PhoneNumber { get; set; }

    [Required]
    public string Role { get; set; }
    [AllowNull]
    public string StatusName { get; set; }
    public int StatusId { get; set; }
    [Required(ErrorMessage = "Это поле обязательно к заполнению!")]
    public string Position { get; set; }

    public string Oldpassword { get; set; }
    public string Newpassword { get; set; }
    public string Confirmation { get; set; }
    public string UserAvatar { get; set; }
}
