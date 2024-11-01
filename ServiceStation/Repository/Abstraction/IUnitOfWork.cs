﻿using ServiceStation.Models.Entities.Implementation;

namespace ServiceStation.Repository.Abstraction;

public interface IUnitOfWork
{
    IGenericRepository<BrandOfVehicle> BrandsOfVehicleRepository { get; }
    
    IGenericRepository<Defect> DefectsRepository { get; }
    
    IGenericRepository<Manufacturer> ManufacturersRepository { get; }
    
    IGenericRepository<Owner> OwnersRepository { get; }
    
    IGenericRepository<Vehicle> VehicleRepository { get; }
}