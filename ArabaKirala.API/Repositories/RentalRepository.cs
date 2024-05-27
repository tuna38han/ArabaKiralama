using ArabaKirala.API.Models;

namespace ArabaKirala.API.Repositories;

public class RentalRepository: BaseRepository<Rental>, IRentalRepository
{
    public RentalRepository(ApplicationDbContext context) : base(context)
    {
    }
}