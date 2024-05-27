using System.Security.Claims;
using ArabaKirala.API.Dtos.Rental;
using ArabaKirala.API.Dtos.User;
using ArabaKirala.API.Models;
using ArabaKirala.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArabaKirala.API.Controllers
{
    [Route("api/[action]/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRentalRepository _rentalRepository;
        private readonly ICarRepository _carRepository;

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IRentalRepository rentalRepository, ICarRepository carRepository)
        {
            _userManager = userManager;
            _rentalRepository = rentalRepository;
            _carRepository = carRepository;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUser dto)
        {
            var user = new AppUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.FirstName + dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Role = "User"
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            return Ok(result.Succeeded);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateRental(CreateRentalDto dto)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (dto.RentalStartDate < DateTime.Today)
            {
                return BadRequest("Başlangıç tarihi bugünden önce olamaz.");
            }

            if (dto.RentalEndDate < dto.RentalStartDate)
            {
                return BadRequest("Bitiş tarihi başlangıç tarihinden önce olamaz.");
            }

            var rental = new Rental()
            {
                UserId = userId,
                CarId = dto.CarId,
                RentalStartDate = dto.RentalStartDate,
                RentalEndDate = dto.RentalEndDate,
                Location = dto.Location,
                DeliveryLocation = dto.DeliveryLocation,
                Status = true
            };

            await _rentalRepository.AddAsync(rental);
            await _rentalRepository.SaveAsync();

            var car = await _carRepository.GetByIdAsync(dto.CarId.ToString());
            if (car != null)
            {
                car.Status = true;
                _carRepository.Update(car);
                await _carRepository.SaveAsync();
            }

            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyRentals()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return BadRequest("User not found.");
            }

            var myRentals = await _rentalRepository.GetAll()
                .Include(r => r.Car)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Status)
                .ToListAsync();

            return Ok(myRentals);
        }

        [HttpGet]
        public IActionResult GetAllListCar()
        {
            return Ok(_carRepository.GetAll().ToList());
        }

        [HttpGet]
        public IActionResult GetAllListAvailableCar()
        {
            return Ok(_carRepository.GetAll().Where(x => x.Status == false).ToList());
        }


        [HttpGet("{carId}")]
        public async Task<IActionResult> GetCar(string carId)
        {
            var car = await _carRepository.GetByIdAsync(carId);
            return Ok(car);
        }

        [HttpPut("{rentalId}")]
        [Authorize]
        public async Task<IActionResult> ExtendRental(string rentalId, ExtendRentalDto dto)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var rental = await _rentalRepository.GetByIdAsync(rentalId);

            if (rental == null)
            {
                return NotFound();
            }

            if (rental.UserId != userId)
            {
                return Forbid();
            }

            if (rental.RentalEndDate >= dto.NewRentalEndDate)
            {
                return BadRequest();
            }

            rental.RentalEndDate = dto.NewRentalEndDate;

            _rentalRepository.Update(rental);
            await _rentalRepository.SaveAsync();

            return Ok();
        }

        [HttpPost("{rentalId}")]
        [Authorize]
        public async Task<IActionResult> CompleteRental(string rentalId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest("User not found.");
            }

            var rental = await _rentalRepository.GetByIdAsync(rentalId);

            if (rental == null)
            {
                return BadRequest("Rental not found.");
            }

            if (rental.UserId != userId)
            {
                return Unauthorized("You are not authorized to complete this rental.");
            }

            rental.Status = false;
            rental.UpdatedDate = DateTime.Now;
            _rentalRepository.Update(rental);
            await _rentalRepository.SaveAsync();
            var car = await _carRepository.GetByIdAsync(rental.CarId.ToString());
            if (car != null)
            {
                car.Status = false;
                _carRepository.Update(car);
                await _carRepository.SaveAsync();
            }

            return Ok("Rental completed successfully.");
        }
    }
}