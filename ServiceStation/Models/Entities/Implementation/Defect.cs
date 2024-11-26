using ServiceStation.Models.Entities.Abstraction;

namespace ServiceStation.Models.Entities.Implementation;

public class Defect : AbstractEntity
{
    public Guid VehicleId { get; set; }
    
    public Guid WorkerId { get; set; }
    
    public string Fault { get; set; }
    
    public string? Description { get; set; }
    
    public bool IsFixed { get; set; }
    
    /*public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; } //TODO переименовать*/
}