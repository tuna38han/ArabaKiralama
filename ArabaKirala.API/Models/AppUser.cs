using Microsoft.AspNetCore.Identity;

namespace ArabaKirala.API.Models;

public class AppUser: IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Role { get; set; }
    public ICollection<Rental> Rentals { get; set; }
}