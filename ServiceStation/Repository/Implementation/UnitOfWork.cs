using ServiceStation.Models.Entities.Implementation;
using ServiceStation.Repository.Abstraction;

namespace ServiceStation.Repository.Implementation;

public class UnitOfWork(DataBaseContext context) : IUnitOfWork
{
    private IGenericRepository<BrandOfVehicle>? _brandsOfVehicleRepository;
    private IGenericRepository<Defect>? _defectsRepository;
    private IGenericRepository<Manufacturer>? _manufacturersRepository;
    private IGenericRepository<Owner>? _ownersRepository;
    private IGenericRepository<Vehicle>? _vehicleRepository;
    
    public IGenericRepository<BrandOfVehicle> BrandsOfVehicleRepository =>
        _brandsOfVehicleRepository ?? new GenericRepository<BrandOfVehicle>(context);
    
    public IGenericRepository<Defect> DefectsRepository =>
        _defectsRepository ?? new GenericRepository<Defect>(context);
    
    public IGenericRepository<Manufacturer> ManufacturersRepository =>
        _manufacturersRepository ?? new GenericRepository<Manufacturer>(context);
    
    public IGenericRepository<Owner> OwnersRepository =>
        _ownersRepository ?? new GenericRepository<Owner>(context);
    
    public IGenericRepository<Vehicle> VehicleRepository =>
        _vehicleRepository ?? new GenericRepository<Vehicle>(context);
}