﻿using ServiceStation.Models.Entities.Abstraction;

namespace ServiceStation.Models.Entities.Implementation;

public class Vehicle : AbstractEntity
{
    public string OwnerId { get; set; }
    
    //TODO разобраться нужны ли оба свойства ниже
    public string ManufacturerId { get; set; }
    
    public string BrandOfVehicleId { get; set; }
    
    public DateOnly YearOfRelease { get; set; }
    
    public string LicenseNumber { get; set; }
    
    //TODO мб поставить statusEnum
    public string Status { get; set; }
    
    public DateTime ServiceCallDate { get; set; }
    
    public virtual Owner Owner { get; set; }
    
    public virtual Manufacturer Manufacturer { get; set; }
    
    public virtual BrandOfVehicle BrandOfVehicle { get; set; }
}