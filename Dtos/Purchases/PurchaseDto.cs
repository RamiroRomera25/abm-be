namespace technical_tests_backend_ssr.Dtos;

public class PurchaseDto
{
    public Guid PurchaseId { get; set; }
    
    public string Title { get; set; }
    
    public double TargetPrice { get; set; }
    
    public double MoneyCollected { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    public int Stock { get; set; }
    
    public bool IsActive { get; set; }
    
    public virtual List<MovementDto> movements { get; set; }
}