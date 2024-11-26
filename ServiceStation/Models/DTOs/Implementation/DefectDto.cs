using ServiceStation.Models.DTOs.Abstraction;

namespace ServiceStation.Models.DTOs.Implementation;

public class DefectDto : AbstractDto
{
    public Guid Id { get; set; }
    
    public Guid VehicleId { get; set; }
    
    public Guid WorkerId { get; set; }
    
    public string Fault { get; set; }
    
    public string? Description { get; set; }
    
    public bool IsFixed { get; set; }
    
    public string BackgroundColor { get; set; }
    
    /*public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; } //TODO переименовать*/
}