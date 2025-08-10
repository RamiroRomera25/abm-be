namespace technical_tests_backend_ssr.Dtos;

public class AuctionDtoPost
{
    public string Title { get; set; }
    
    public double InitialPrice { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
}