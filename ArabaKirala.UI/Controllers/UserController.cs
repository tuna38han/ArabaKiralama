using Microsoft.AspNetCore.Mvc;

namespace ArabaKirala.UI.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult GetAllCars()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateRental()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult GetMyRentals()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetCarDetailById()
        {
            return View();
        }
    }
}