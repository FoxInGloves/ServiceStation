using System.Globalization;
using Microsoft.EntityFrameworkCore;
using ServiceStation.Models.Entities.Implementation;
using static System.String;

namespace ServiceStation.Repository;

public sealed class ApplicationDatabaseContext : DbContext
{
    public ApplicationDatabaseContext()
    {
    }

    public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString =
            "Host=localhost;Port=5432;Database=service_station;Username=postgres;Password=123456789";

        optionsBuilder.UseLazyLoadingProxies().UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BrandOfVehicle>().HasKey(o => o.Id);
        modelBuilder.Entity<Defect>().HasKey(o => o.Id);
        modelBuilder.Entity<ModelOfVehicle>().HasKey(o => o.Id);
        modelBuilder.Entity<Owner>().HasKey(o => o.Id);
        modelBuilder.Entity<Status>().HasKey(o => o.Id);
        modelBuilder.Entity<Vehicle>().HasKey(o => o.Id);
        modelBuilder.Entity<Worker>().HasKey(o => o.Id);

        modelBuilder.Entity<BrandOfVehicle>().HasData(
            new BrandOfVehicle
            {
                Id = new Guid("F4392198-F9CA-40B0-9955-D988B93CC400"), //New id
                Name = "Lada"
            },
            new BrandOfVehicle
            {
                Id = new Guid("4F2AB256-9DFE-4F79-9A3D-4299F49EDE47"), //New id
                Name = "Toyota"
            });
        modelBuilder.Entity<ModelOfVehicle>().HasData(
            new ModelOfVehicle
            {
                Id = new Guid("D87C29CC-1CF3-4B37-847B-9977E3E7A834"), //New id
                BrandId = new Guid("4F2AB256-9DFE-4F79-9A3D-4299F49EDE47"), //Toyota
                Name = "Mark II"
            },
            new ModelOfVehicle
            {
                Id = new Guid("53DC0E0B-6D8A-439C-8B28-DCFEC210B55A"), //New id
                BrandId = new Guid("F4392198-F9CA-40B0-9955-D988B93CC400"), //Lada
                Name = "Vesta"
            });
        modelBuilder.Entity<Owner>().HasData(
            new Owner
            {
                Id = new Guid("31EB1A61-48C4-4091-92C7-890044910EB9"), //New id
                FirstName = "Ryan",
                LastName = "Gosling",
                MiddleName = Empty,
                City = "Moscow",
                Street = "Sadovaya",
                BuildingNumber = "13"
            });
        modelBuilder.Entity<Status>().HasData(
            new Status
            {
                Id = new Guid("5EC8A0CC-0BE2-4567-99B5-1FD6C1EE56F9"),
                Name = "В работе",
                Color = "#f0b71d"
            },
            new Status
            {
                Id = new Guid("21AAE3F2-336D-49E6-92AF-0507D2375D1C"),
                Name = "Закончено",
                Color = "#31d650"
            },
            new Status
            {
                Id = new Guid("BA62C8B1-888D-49D5-95A7-54174E753DB0"),
                Name = "Отложено",
                Color = "#eb1e1e"
            });
        modelBuilder.Entity<Vehicle>().HasData(
            new Vehicle
            {
                Id = new Guid("5316A5E7-FD12-4739-989B-18A8E75FE5EA"), //New id
                OwnerId = new Guid("31EB1A61-48C4-4091-92C7-890044910EB9"), //Ryan Gosling
                ModelOfVehicleId = new Guid("53DC0E0B-6D8A-439C-8B28-DCFEC210B55A"), //Vesta
                YearOfRelease = "2022",
                RegistrationNumber = "1",
                StatusId = new Guid("5EC8A0CC-0BE2-4567-99B5-1FD6C1EE56F9"), //В работе
                ServiceCallDate = DateOnly.FromDateTime(DateTime.Now).ToString()
            });
        modelBuilder.Entity<Defect>().HasData(
            new Defect
            {
                Id = new Guid("454DE61E-627D-46ED-851B-9A97BBFA571F"), //New id
                VehicleId = new Guid("5316A5E7-FD12-4739-989B-18A8E75FE5EA"), //Vesta
                Fault = "Engine",
                Description = "none",
                IsFixed = false,
                StartDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                EndDate = null
            });
        modelBuilder.Entity<Worker>().HasData(
            new Worker
            {
                Id = new Guid("D5C60564-6241-49BC-9C26-A7105FF1B4A9"), //New id
                VehicleId = new Guid("5316A5E7-FD12-4739-989B-18A8E75FE5EA"), //Vesta
                //DefectId = new Guid("454DE61E-627D-46ED-851B-9A97BBFA571F"), //Defect engine
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