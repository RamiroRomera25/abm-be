using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Models.Enums;

namespace technical_tests_backend_ssr.Dtos;

public class MovementDto
{
    public Guid MovementId { get; set; }

    public string Owner { get; set; }

    public Double Cost { get; set; }
    
    public string Comments { get; set; }
    
    public DateTime Date { get; set; }
    
    public MovementType MovementType { get; set; }
    
    public bool IsActive { get; set; }
    
    public virtual AuctionDto Auction { get; set; } = null!;

    public virtual PurchaseDto Purchase { get; set; } = null!;
}