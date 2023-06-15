using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace Make_a_Drop.MVC.Models
{
    public class LoginModel
    {

        [Required]
        public string? Username { get; set; }

       
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }
    }
}
