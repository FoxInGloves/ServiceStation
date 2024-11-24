using ServiceStation.Models.Entities.Abstraction;

namespace ServiceStation.Models.Entities.Implementation;

public class ModelOfVehicle : AbstractEntity
{
    public Guid BrandId { get; set; }
    
    public string Name { get; set; }
    
    public virtual BrandOfVehicle? Brand { get; set; }
}