using System.Globalization;
using Microsoft.EntityFrameworkCore;
using ServiceStation.Models.Entities.Implementation;
using static System.String;

namespace ServiceStation.Repository;

public sealed class ApplicationDatabaseContext : DbContext
{
    public ApplicationDatabaseContext() { }
    
    public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString = "Host=localhost;Port=5432;Database=service_station;Username=postgres;Password=123456789";
        
        optionsBuilder.UseLazyLoadingProxies().UseNpgsql(connectionString);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BrandOfVehicle>().HasKey(o => o.Id);
        modelBuilder.Entity<Defect>().HasKey(o => o.Id);
        modelBuilder.Entity<ModelOfVehicle>().HasKey(o => o.Id);
        modelBuilder.Entity<Owner>().HasKey(o => o.Id);
        
        modelBuilder.Entity<Vehicle>().HasKey(o => o.Id);
        
        
        modelBuilder.Entity<Worker>().HasKey(o => o.Id);

        modelBuilder.Entity<BrandOfVehicle>().HasData(
            new BrandOfVehicle 
            {
                Id = new Guid("F4392198-F9CA-40B0-9955-D988B93CC400"), 
                Name = "Lada"
            },
            new BrandOfVehicle
            {
                Id = new Guid("4F2AB256-9DFE-4F79-9A3D-4299F49EDE47"),
                Name = "Toyota"
            });
        modelBuilder.Entity<ModelOfVehicle>().HasData(
            new ModelOfVehicle
            {
                Id = new Guid("D87C29CC-1CF3-4B37-847B-9977E3E7A834"),
                BrandId = new Guid("4F2AB256-9DFE-4F79-9A3D-4299F49EDE47"),
                Name = "Mark II"
            },
            new ModelOfVehicle
            {
                Id = new Guid("53DC0E0B-6D8A-439C-8B28-DCFEC210B55A"),
                BrandId = new Guid("F4392198-F9CA-40B0-9955-D988B93CC400"),
                Name = "Vesta"
            });
        modelBuilder.Entity<Defect>().HasData(
            new Defect
            {
                Id = new Guid("454DE61E-627D-46ED-851B-9A97BBFA571F"),
                VehicleId = new Guid("5316A5E7-FD12-4739-989B-18A8E75FE5EA"),
                Fault = "Engine",
                Description = "none",
                EliminationTime = Empty,
                StartDate = Empty
            });
        modelBuilder.Entity<Owner>().HasData(
            new Owner
            {
                Id = new Guid("31EB1A61-48C4-4091-92C7-890044910EB9"),
                FirstName = "Ryan",
                LastName = "Gosling",
                MiddleName = Empty,
                City = "Moscow",
                Street = "Sadovaya",
                BuildingNumber = "13"
            });
        modelBuilder.Entity<Vehicle>().HasData(
            new Vehicle
            {
                Id = new Guid("5316A5E7-FD12-4739-989B-18A8E75FE5EA"),
                OwnerId = new Guid("31EB1A61-48C4-4091-92C7-890044910EB9"),
                ModelOfVehicleId = new Guid("53DC0E0B-6D8A-439C-8B28-DCFEC210B55A"),
                YearOfRelease = "2022",
                LicenseNumber = "1",
                Status = "Delayed",
                ServiceCallDate = DateTime.Today.ToString(CultureInfo.CurrentCulture)
            });
        modelBuilder.Entity<Worker>().HasData(
            new Worker
            {
                Id = new Guid("D5C60564-6241-49BC-9C26-A7105FF1B4A9"),
                VehicleId = new Guid("5316A5E7-FD12-4739-989B-18A8E75FE5EA"),
                DefectId = new Guid("454DE61E-627D-46ED-851B-9A97BBFA571F"),
                FirstName = "Morgan",
                LastName = "Freeman",
            });
    }
    
    public DbSet<BrandOfVehicle> BrandsOfVehicle { get; init; }
    
    public DbSet<Defect> Defects { get; init; }
    
    public DbSet<ModelOfVehicle> ModelOfVehicle { get; init; }
    
    public DbSet<Worker> Workers { get; init; }
    
    public DbSet<Owner> Owners { get; init; }    
    
    public DbSet<Vehicle> Vehicles { get; init; }
}