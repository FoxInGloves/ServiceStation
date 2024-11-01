using ServiceStation.Models.Entities.Abstraction;

namespace ServiceStation.Models.Entities.Implementation;

public class Manufacturer : AbstractEntity
{
    public string Name { get; set; }
}