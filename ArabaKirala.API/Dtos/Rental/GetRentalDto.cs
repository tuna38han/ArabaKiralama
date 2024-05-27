namespace ArabaKirala.API.Dtos.Rental;

public class GetRentalDto
{
    public string Id { get; set; }
    public string CarBrandName { get; set; }
    public string CarName { get; set; }
    public string UserName { get; set; }
    public string Location { get; set; }
    
    public string DeliveryLocation { get; set; }
    public float Price { get; set; }
    public bool Status { get; set; }
    public DateTime RentalStartDate { get; set; }
    public DateTime RentalEndDate { get; set; }
}