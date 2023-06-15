#nullable disable

using Make_a_Drop.Application.Services;
using Make_a_Drop.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Make_a_Drop.Application.Models.User;
using Microsoft.AspNetCore.Identity;
using Make_a_Drop.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Make_a_Drop.MVC.Filters;
using Make_a_Drop.Shared.Services;
using Make_a_Drop.Application.Exceptions;
using System.Web.WebPages;
using FluentValidation;

namespace Make_a_Drop.MVC.Controllers
{

    public class AccountController : Controller
    {

        private readonly IUserService _userService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IClaimService _claimService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IValidator<CreateUserModel> _validator;

        public AccountController(IUserService userService,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IClaimService claimService,
            IMapper mapper,
            ILoggerFactory loggerFactory,
            IValidator<CreateUserModel> validator
            )
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
            _claimService = claimService;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _validator = validator;


        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CreateUserModel model)
        {
            var result = await _userService.CreateAsync(model);                              
            return RedirectToAction(nameof(Login));               
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            model.ReturnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(model.ReturnUrl);
                }
                else
                {
                    ViewBag.Message = "Invalid login attempt.";
                    return View();
                }
            }     
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(Index), "Home");
        }

        [CustomAuthorize]
        [HttpGet("Account/Details")]
        public async Task<IActionResult> Details()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return View(user);
        }

        [CustomAuthorize]
        [HttpGet("Account/Delete/{guid}")]
        public async Task<IActionResult> Delete(string guid)
        {
            return View(await _userService.GetByIdAsync(guid));
        }

        [CustomAuthorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string guid)
        {
            await _userService.DeleteAsync(guid);
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }


        [CustomAuthorize]
        [HttpGet("Account/Edit/{guid}")]
        public async Task<IActionResult> Edit(string guid)
        {
            return View(_mapper.Map<UpdateModel>(await _userService.GetByIdAsync(guid)));
        }

        [CustomAuthorize]
        [HttpPost("Account/Edit/{guid}"), ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid guid, UpdateModel user)
        {            
            if (guid != user.Id)
            {
                throw new NotFoundException("User not found");
            }
            if (ModelState.IsValid)
            {
                await _userService.UpdateAsync(user);
                return RedirectToAction(nameof(Details));
            }
            return View(user);
        }

        [CustomAuthorize]
        [HttpGet("Account/ChangePassword/{guid}")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [CustomAuthorize]
        [HttpPost("Account/ChangePassword/{guid}"), ActionName("ChangePassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string guid, ChangePasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(guid);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            if (ModelState.IsValid)
            {
                var result = await _userService.ChangePasswordAsync(guid, model);                
                return RedirectToAction(nameof(Details));                               
            }
            return View(model);
        }
    }
}
