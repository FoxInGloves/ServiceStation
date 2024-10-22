using ServiceStation.Models.Abstraction;

namespace ServiceStation.Models.Implementation;

public class BrandOfVehicle : Entity
{
    public string Brand { get; set; }
    
    public string Model { get; set; }
}