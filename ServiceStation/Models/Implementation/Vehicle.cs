using ServiceStation.Models.Abstraction;

namespace ServiceStation.Models.Implementation;

public class Vehicle : Entity
{
    public string OwnerId { get; set; }
    
    public string ManufacturerId { get; set; }
    
    public string BrandOfVehicleId { get; set; }
    
    public string YearOfRelease { get; set; }
    
    public string LicenseNumber { get; set; }
}