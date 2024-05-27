namespace ArabaKirala.API.Dtos.Rental;

public class UpdateRentalDto
{
    public string Id { get; set; }
    public DateTime RentalStartDate { get; set; }
    public DateTime RentalEndDate { get; set; }
}