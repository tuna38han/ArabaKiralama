using Microsoft.AspNetCore.Mvc;

namespace ArabaKirala.UI.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateCar()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateCar()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllListAvailableCar()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllListActiveRental()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllListRental()
        {
            return View();
        }
    }
}