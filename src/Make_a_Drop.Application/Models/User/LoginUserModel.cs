#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Make_a_Drop.Application.Models.User;

public class LoginUserModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }

}

