using ServiceStation.Models.DTOs.Abstraction;

namespace ServiceStation.Models.DTOs.Implementation;

public class WorkerDto : AbstractDto
{
    public Guid Id { get; set; }
    
    public Guid VehicleId { get; set; }
    
    public Guid DefectId { get; set; }
    
    public string FullName { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string? MiddleName { get; set; }
}