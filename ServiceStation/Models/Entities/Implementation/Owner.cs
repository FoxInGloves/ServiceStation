using ServiceStation.Models.Entities.Abstraction;

namespace ServiceStation.Models.Entities.Implementation;

public class Owner : AbstractEntity
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string MiddleName { get; set; }
    
    public string City { get; set; }
    
    public string Street { get; set; }
    
    public string BuildingNumber { get; set; }
}