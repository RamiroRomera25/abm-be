using technical_tests_backend_ssr.Models.Enums;

namespace technical_tests_backend_ssr.Dtos;

public class MovementDtoPost
{
    public Double Cost { get; set; }
    
    public string Comments { get; set; }
    
    public DateTime Date { get; set; }
    public MovementType MovementType { get; set; }

    public Guid? PurchaseId { get; set; }
    public Guid? AuctionId { get; set; }
}