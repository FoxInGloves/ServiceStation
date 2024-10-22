using ServiceStation.Models.Abstraction;

namespace ServiceStation.Models.Implementation;

public class Owner : Entity
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string MiddleName { get; set; }
    
    public string City { get; set; }
    
    public string Street { get; set; }
    
    public string BuildingNumber { get; set; }
}