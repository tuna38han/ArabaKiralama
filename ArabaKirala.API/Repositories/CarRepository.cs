using ArabaKirala.API.Models;

namespace ArabaKirala.API.Repositories;

public class CarRepository: BaseRepository<Car>, ICarRepository
{
    public CarRepository(ApplicationDbContext context) : base(context)
    {
    }
}