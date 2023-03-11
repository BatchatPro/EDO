using System.ComponentModel.DataAnnotations;

namespace EDO.Access.DTO;

public class LogInDTO
{
    [Required(ErrorMessage = "This Poly is Required.")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "This Poly is Required.")]
    public string Password { get; set; }
}
