using ServiceStation.Models.Entities.Implementation;

namespace ServiceStation.Repository.Abstraction;

public interface IUnitOfWork
{
    IGenericRepository<BrandOfVehicle> BrandsOfVehicleRepository { get; }
    
    IGenericRepository<Defect> DefectsRepository { get; }
    
    IGenericRepository<ModelOfVehicle> ModelsRepository { get; }
    
    IGenericRepository<Owner> OwnersRepository { get; }
    
    IGenericRepository<Vehicle> VehicleRepository { get; }
    
    IGenericRepository<Worker> WorkersRepository { get; }
    
    IGenericRepository<Status> StatusRepository { get; }
    
    Task SaveChangesAsync();
}