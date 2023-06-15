using System.ComponentModel.DataAnnotations;
using Make_a_Drop.Application.Models;

namespace Make_a_Drop.MVC.Models;

public class RegisterModel : BaseResponseModel
{

    [Required]
    public string? FirstName { get; set; }

    [Required]
    public string? LastName { get; set; }

    [Required]
    public string? Username { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required]
    [Compare(nameof(Password))]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }
}

