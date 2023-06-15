using Microsoft.AspNetCore.Mvc;
using Make_a_Drop.MVC.Filters;
using Make_a_Drop.Application.Services;

namespace Make_a_Drop.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IFirebaseStorageService _firebaseStorageService;
    private readonly IConfiguration _configuration;

    public HomeController(ILogger<HomeController> logger, IFirebaseStorageService firebaseStorageService, 
        IConfiguration configuration)
    {
        _logger = logger;
        _firebaseStorageService = firebaseStorageService;
        _configuration = configuration;
    }


    public IActionResult Index()
    {
        return View();
    }

    [CustomAuthorize]
    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
}

