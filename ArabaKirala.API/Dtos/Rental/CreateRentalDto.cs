namespace ArabaKirala.API.Dtos.Rental;

public class CreateRentalDto
{
    public int CarId { get; set; }
    public string Location { get; set; }
    
    public string DeliveryLocation { get; set; }
    public DateTime RentalStartDate { get; set; }
    public DateTime RentalEndDate { get; set; }
}