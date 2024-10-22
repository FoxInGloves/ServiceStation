using ServiceStation.Models.Abstraction;

namespace ServiceStation.Models.Implementation;

public class Defect : Entity
{
    public string VehicleId { get; set; }
    
    public string Fault { get; set; }
    
    public string EliminationTime { get; set; }
}