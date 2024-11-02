using ServiceStation.Models.DTOs.Abstraction;
using ServiceStation.Models.Entities.Implementation;

namespace ServiceStation.Models.DTOs.Implementation;

public class VehicleDto : AbstractDto
{
    public string BrandAndModel { get; set; }
    
    public string OwnerName { get; set; }
    
    public DateOnly YearOfRelease { get; set; }
    
    public string LicenseNumber { get; set; }
    
    public string Status { get; set; }
    
    public string StatusColor { get; set; }
    
    public DateTime ServiceCallDate { get; set; }
    
    public ICollection<Worker> CollectionOfWorkers { get; set; }
    
    public ICollection<Defect> CollectionsOfDefects { get; set; }
}