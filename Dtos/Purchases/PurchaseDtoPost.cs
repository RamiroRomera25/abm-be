namespace technical_tests_backend_ssr.Dtos;

public class PurchaseDtoPost
{
    public string Title { get; set; }
    
    public double TargetPrice { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    public int Stock { get; set; }
}