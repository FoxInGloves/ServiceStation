using ServiceStation.Models.DTOs.Abstraction;
using ServiceStation.Models.Entities.Implementation;

namespace ServiceStation.Models.DTOs.Implementation;

public class VehicleDto : AbstractDto
{
    public Guid Id { get; set; }
    
    public Guid OwnerId { get; set; }
    
    public Guid StatusId { get; set; }
    
    public string BrandAndModel { get; set; }
    
    public string? OwnerName { get; set; }
    
    public int YearOfRelease { get; set; }
    
    public string RegistrationNumber { get; set; }
    
    public DateOnly ServiceCallDate { get; set; }
    
    public int ElapsedDays { get; set; }
    
    public OwnerDto? Owner { get; set; }
    
    public Status? Status { get; set; }
    
    public ModelOfVehicle? ModelOfVehicle { get; set; }
    
    public IEnumerable<WorkerDto>? CollectionOfWorkers { get; set; }
    
    public IEnumerable<DefectDto>? CollectionsOfDefects { get; set; }
}