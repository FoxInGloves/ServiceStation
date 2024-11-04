using ServiceStation.Models.Entities.Implementation;
using ServiceStation.Repository.Abstraction;

namespace ServiceStation.Repository.Implementation;

public class UnitOfWork(ApplicationDatabaseContext context) : IUnitOfWork
{
    private IGenericRepository<BrandOfVehicle>? _brandsOfVehicleRepository;
    private IGenericRepository<Defect>? _defectsRepository;
    private IGenericRepository<ModelOfVehicle>? _modelsRepository;
    private IGenericRepository<Worker>? _workersRepository;
    private IGenericRepository<Owner>? _ownersRepository;
    private IGenericRepository<Vehicle>? _vehicleRepository;
    
    public IGenericRepository<BrandOfVehicle> BrandsOfVehicleRepository =>
        _brandsOfVehicleRepository ?? new GenericRepository<BrandOfVehicle>(context);
    
    public IGenericRepository<Defect> DefectsRepository =>
        _defectsRepository ?? new GenericRepository<Defect>(context);
    
    public IGenericRepository<ModelOfVehicle> ModelsRepository =>
    _modelsRepository ?? new GenericRepository<ModelOfVehicle>(context);
    
    public IGenericRepository<Owner> OwnersRepository =>
        _ownersRepository ?? new GenericRepository<Owner>(context);
    
    public IGenericRepository<Vehicle> VehicleRepository =>
        _vehicleRepository ?? new GenericRepository<Vehicle>(context);
    
    public IGenericRepository<Worker> WorkersRepository =>
        _workersRepository ?? new GenericRepository<Worker>(context);
}