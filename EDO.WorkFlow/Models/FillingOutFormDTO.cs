using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EDO.WorkFlow.FillingOutForm;

public class FillingOutFormDTO
{
    [Required]
    public string Executor { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "Пароль должен содержать не менее 6 символов!", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Confirming { get; set; }
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
    public string Checker { get; set; }
    [Required(ErrorMessage = "Это поле обязательно к заполнению!")]
    [StringLength(50, ErrorMessage = "Фамилия превышает 50 символов!")]
    public string Towhom { get; set; }
    [Required(ErrorMessage = "Это поле обязательно к заполнению!")]
    [StringLength(50, ErrorMessage = "Имя превышает 50 символов!")]
    public string Contentoftheletter { get; set; }
    [AllowNull]
    public string MiddelName { get; set; }
   
}
