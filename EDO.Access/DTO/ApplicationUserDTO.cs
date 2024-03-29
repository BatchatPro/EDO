﻿using NullGuard;
using System.ComponentModel.DataAnnotations;

namespace EDO.Access.DTO;

public class ApplicationUserDTO
{
    public string Id { get; set; }
    [Required(ErrorMessage = "This Field Requider!")]
    public string? UserName { get; set; }
    [Required(ErrorMessage = "This Field Requider!")]
    public string? LastName { get; set; }
    [Required(ErrorMessage = "This Field Requider!")]
    public string? FirstName { get; set; }
    [Required(ErrorMessage = "This Field Requider!")]
    public string? ThirdName { get; set; }
    [Required(ErrorMessage = "This Field Requider!")]
    public string PhoneNumber { get; set; }
    [AllowNull]
    public string? Email { get; set; }
}
