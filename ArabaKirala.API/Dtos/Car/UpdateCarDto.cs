namespace ArabaKirala.API.Dtos.Car;

public class UpdateCarDto
{
    public string Id { get; set; }
    public string Luggage { get; set; }
    public string Fuel { get; set; }
    public string Description { get; set; }
    public string Transmission { get; set; }
    public float Mil { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public float Price { get; set; }
    public IFormFile? ImageFile { get; set; }
}