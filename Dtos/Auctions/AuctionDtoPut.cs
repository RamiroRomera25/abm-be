namespace technical_tests_backend_ssr.Dtos;

public class AuctionDtoPut
{
    public Guid AuctionId { get; set; }
    
    public string Title { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
}