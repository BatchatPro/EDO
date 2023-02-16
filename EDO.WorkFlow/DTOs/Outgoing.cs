using System.ComponentModel.DataAnnotations;

namespace EDO.WorkFlow.DTOs;

public class Outgoing
{
    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; }

    [Required]
    public string Towhom { get; set; }

    public string Shippingstatus { get; set; }

    public string Description { get; set; }

    public string Data { get; set; }
}
