using ServiceStation.Models.Entities.Abstraction;

namespace ServiceStation.Models.Entities.Implementation;

public class Status : AbstractEntity
{
    public string Name { get; set; }
    
    public string Color { get; set; }
}