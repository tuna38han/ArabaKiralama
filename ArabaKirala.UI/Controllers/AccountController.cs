using Microsoft.AspNetCore.Mvc;

namespace ArabaKirala.UI.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }
    }
}