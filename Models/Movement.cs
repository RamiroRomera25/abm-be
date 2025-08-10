using System.Text.Json.Serialization;
using technical_tests_backend_ssr.Models.Enums;

namespace technical_tests_backend_ssr.Models;

public class Movement
{
    public Guid MovementId { get; set; }
    
    public Double Cost { get; set; }
    
    public string Comments { get; set; }
    
    public DateTime Date { get; set; }
    
    public MovementType MovementType { get; set; }
    
    public bool IsActive { get; set; }
    
    [JsonIgnore]
    public virtual Auction? Auction { get; set; }

    [JsonIgnore]
    public virtual Purchase? Purchase { get; set; }
}