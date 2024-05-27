using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArabaKirala.API.Models;

public class ApplicationDbContext:IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions options):base(options)
    {}

    public DbSet<Car> Cars { get; set; }
    public DbSet<Rental> Rentals { get; set; }
}