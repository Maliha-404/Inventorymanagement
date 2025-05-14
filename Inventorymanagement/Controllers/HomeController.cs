using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Inventorymanagement.Models;

namespace Inventorymanagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Redirect to UsersController's Index action
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Users");  // Redirects to the Users Index page
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
