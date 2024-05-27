using ArabaKirala.API.Dtos.Car;
using ArabaKirala.API.Dtos.Rental;
using ArabaKirala.API.Models;
using ArabaKirala.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArabaKirala.API.Controllers
{
    [Route("api/[action]/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IWebHostEnvironment _environment;
        public AdminController(ICarRepository carRepository, IRentalRepository rentalRepository, IWebHostEnvironment environment)
        {
            _carRepository = carRepository;
            _rentalRepository = rentalRepository;
            _environment = environment;
        }


        [HttpGet]
        public IActionResult GetAllListCar()
        {
            return Ok(_carRepository.GetAll().ToList());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar([FromForm] CreateCarDto dto)
        {
            var car = new Car()
            {
                Name = dto.Name,
                Luggage = dto.Luggage,
                Fuel = dto.Fuel,
                Description = dto.Description,
                Transmission = dto.Transmission,
                Mil = dto.Mil,
                Brand = dto.Brand,
                Price = dto.Price,
                Status = false
            };

            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "_" + dto.ImageFile.FileName;
                var filePath = Path.Combine("img", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(fileStream);
                }

                car.ImageUrl = "/img/" + fileName;
                await CopyFileToMvcProject(filePath, fileName);
            }

            await _carRepository.AddAsync(car);
            await _carRepository.SaveAsync();

            return Ok();
        }

        private async Task CopyFileToMvcProject(string sourceFilePath, string fileName)
        {
            var mvcTargetDirectory = @"../../ArabaKirala/ArabaKirala.UI/wwwroot/img";
            var mvcTargetFilePath = Path.Combine(mvcTargetDirectory, fileName);

            using (var sourceStream = new FileStream(sourceFilePath, FileMode.Open))
            {
                using (var targetStream = new FileStream(mvcTargetFilePath, FileMode.Create))
                {
                    await sourceStream.CopyToAsync(targetStream);
                }
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar([FromForm] UpdateCarDto dto)
        {
            var car = await _carRepository.GetByIdAsync(dto.Id);
            if (car == null)
            {
                return NotFound();
            }

            car.Name = dto.Name;
            car.Luggage = dto.Luggage;
            car.Brand = dto.Brand;
            car.Fuel = dto.Fuel;
            car.Description = dto.Description;
            car.Mil = dto.Mil;
            car.Transmission = dto.Transmission;
            car.Brand = dto.Brand;
            car.Price = dto.Price;
            car.UpdatedDate = DateTime.Now;

            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "_" + dto.ImageFile.FileName;
                var filePath = Path.Combine("img", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(fileStream);
                }

                car.ImageUrl = "/img/" + fileName;
                await CopyFileToMvcProject(filePath, fileName);
            }

            _carRepository.Update(car);
            await _carRepository.SaveAsync();

            return Ok();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(string id)
        {
            await _carRepository.RemoveAsync(id);
            await _carRepository.SaveAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCar(string id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            return Ok(car);
        }


        [HttpGet]
        public IActionResult GetAllListRental()
        {
            var rentals = _rentalRepository.GetAll()
                .OrderByDescending(rental => rental.Status)
                .Select(rental => new GetRentalDto
                {
                    Id = rental.Id.ToString(),
                    CarBrandName = rental.Car.Brand,
                    CarName = rental.Car.Name,
                    UserName = rental.User.FirstName + " " + rental.User.LastName,
                    RentalStartDate = rental.RentalStartDate,
                    RentalEndDate = rental.RentalEndDate,
                    Price = rental.Car.Price,
                    Status = rental.Status,
                    Location = rental.Location,
                    DeliveryLocation = rental.DeliveryLocation
                });

            return Ok(rentals);
        }

        [HttpGet]
        public IActionResult GetAllListActiveRental()
        {
            var rentals = _rentalRepository.GetAll().Where(x => x.Status == true)
                .Select(rental => new GetRentalDto
                {
                    Id = rental.Id.ToString(),
                    CarBrandName = rental.Car.Brand,
                    CarName = rental.Car.Name,
                    UserName = rental.User.FirstName + " " + rental.User.LastName,
                    RentalStartDate = rental.RentalStartDate,
                    RentalEndDate = rental.RentalEndDate,
                    Price = rental.Car.Price,
                    Status = rental.Status,
                    Location = rental.Location,
                    DeliveryLocation = rental.DeliveryLocation
                });

            return Ok(rentals);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRental(UpdateRentalDto dto)
        {
            var rental = await _rentalRepository.GetByIdAsync(dto.Id);
            rental.RentalStartDate = dto.RentalStartDate;
            rental.RentalEndDate = dto.RentalEndDate;
            rental.UpdatedDate = DateTime.Now;
            _rentalRepository.Update(rental);
            await _rentalRepository.SaveAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRental(string id)
        {
            var rental = await _rentalRepository.GetByIdAsync(id);
            if (rental == null)
            {
                return NotFound();
            }

            var car = await _carRepository.GetByIdAsync(rental.CarId.ToString());
            if (car != null)
            {
                car.Status = false;
                _carRepository.Update(car);
                await _carRepository.SaveAsync();
            }

            await _rentalRepository.RemoveAsync(id);
            await _rentalRepository.SaveAsync();

            return Ok();
        }


        [HttpPost("{rentalId}")]
        public async Task<IActionResult> CompleteRental(string rentalId)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);

            if (rental == null)
            {
                return BadRequest("Rental not found.");
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