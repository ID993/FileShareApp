using Make_a_Drop.Application.Models;
using Make_a_Drop.Application.Services;
using Microsoft.AspNetCore.Mvc;


namespace Make_a_Drop.MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;


        public UsersController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            ViewBag.Users = users;
            return View(users);  
        }

    }
}

