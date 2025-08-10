namespace technical_tests_backend_ssr.Dtos;

public class PurchaseDtoPut
{
    public Guid PurchaseId { get; set; }
    
    public string Title { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    public int Stock { get; set; }
}