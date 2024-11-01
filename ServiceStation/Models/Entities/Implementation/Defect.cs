using ServiceStation.Models.Entities.Abstraction;

namespace ServiceStation.Models.Entities.Implementation;

public class Defect : AbstractEntity
{
    public string VehicleId { get; set; }
    
    public string Fault { get; set; }
    
    public string Description { get; set; }
    
    public string EliminationTime { get; set; }
    
    public string StartDate { get; set; } //TODO переименовать
}