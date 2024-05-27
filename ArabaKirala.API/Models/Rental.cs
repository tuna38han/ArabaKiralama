namespace ArabaKirala.API.Models;

public class Rental : BaseEntity
{
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public int CarId { get; set; }
    public string Location { get; set; }
    public string DeliveryLocation { get; set; }
    public Car Car { get; set; }
    public DateTime RentalStartDate { get; set; }
    public DateTime RentalEndDate { get; set; }
    public bool Status { get; set; }
}