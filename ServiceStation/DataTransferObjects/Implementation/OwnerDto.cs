using ServiceStation.DataTransferObjects.Abstraction;

namespace ServiceStation.DataTransferObjects.Implementation;

public class OwnerDto : AbstractDto
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string? MiddleName { get; set; }
    
    public string? FullName {get; set;}
    
    public string City { get; set; }
    
    public string Street { get; set; }
    
    public string BuildingNumber { get; set; }
}