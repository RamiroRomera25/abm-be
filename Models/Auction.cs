using System.Text.Json.Serialization;

namespace technical_tests_backend_ssr.Models;

public class Auction
{
    public Guid AuctionId { get; set; }
    
    public string Title { get; set; }
    
    public double InitialPrice { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public double CurrentPrice { get; set; }
    
    public string? UserWinner { get; set; }
    
    public bool IsActive { get; set; }
    
    public bool IsOpen { get; set; }
}