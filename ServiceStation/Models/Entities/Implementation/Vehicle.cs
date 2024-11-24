using ServiceStation.Models.Entities.Abstraction;

namespace ServiceStation.Models.Entities.Implementation;

public class Vehicle : AbstractEntity
{
    public Guid OwnerId { get; set; }
    
    public Guid StatusId { get; set; }

    //TODO разобраться нужны ли оба свойства ниже
    public Guid ModelOfVehicleId { get; set; }

    public string YearOfRelease { get; set; }

    public string RegistrationNumber { get; set; }
    
    public string ServiceCallDate { get; set; }

    public virtual Owner? Owner { get; set; }
    
    public virtual Status? Status { get; set; }

    public virtual ModelOfVehicle? ModelOfVehicle { get; set; }

    public virtual IEnumerable<Worker>? CollectionOfWorkers { get; set; }

    public virtual IEnumerable<Defect>? CollectionsOfDefects { get; set; }
}