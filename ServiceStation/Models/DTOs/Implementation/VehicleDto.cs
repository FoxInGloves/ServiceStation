using ServiceStation.Models.DTOs.Abstraction;

namespace ServiceStation.Models.DTOs.Implementation;

public class VehicleDto : AbstractDto
{
    public string Brand { get; set; }
    
    public string Model { get; set; }
    
    public DateOnly YearOfRelease { get; set; }
    
    public string LicenseNumber { get; set; }
    
    public string Status { get; set; }
    
    public string StatusColor { get; set; }
}