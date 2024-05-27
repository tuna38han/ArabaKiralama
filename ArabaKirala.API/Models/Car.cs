using System.ComponentModel.DataAnnotations.Schema;

namespace ArabaKirala.API.Models;

public class Car: BaseEntity
{
    public string Brand { get; set; }
    public string Name { get; set; }
    public float Mil { get; set; }
    public string Transmission { get; set; }
    public string Luggage { get; set; }
    public string Fuel { get; set; }
    public string Description { get; set; }
    public bool Status { get; set; }
    public float Price { get; set; }
    public string? ImageUrl { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
}