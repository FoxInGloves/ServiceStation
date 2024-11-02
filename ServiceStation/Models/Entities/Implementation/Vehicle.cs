using ServiceStation.Models.Entities.Abstraction;

namespace ServiceStation.Models.Entities.Implementation;

public class Vehicle : AbstractEntity
{
    public Guid OwnerId { get; set; }
    
    //TODO разобраться нужны ли оба свойства ниже
    public Guid ModelId { get; set; }
    
    public string YearOfRelease { get; set; }
    
    public string LicenseNumber { get; set; }
    
    //TODO мб поставить statusEnum
    public string Status { get; set; }
    
    public string ServiceCallDate { get; set; }
    
    public virtual Owner Owner { get; set; }
    
    public virtual ICollection<Worker> CollectionOfWorkers { get; set; }
    
    public virtual ICollection<Defect> CollectionsOfDefects { get; set; }
    
    public virtual ModelOfVehicle ModelOfVehicle { get; set; }
}