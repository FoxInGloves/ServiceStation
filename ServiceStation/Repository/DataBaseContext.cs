using Microsoft.EntityFrameworkCore;
using ServiceStation.Models.Implementation;

namespace ServiceStation.Repository;

public class DataBaseContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString = "Host=localhost;Port=5432;Database=Temp;Username=postgres;Password=123456789";
        
        optionsBuilder.UseNpgsql(connectionString);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BrandOfVehicle>().HasKey(o => o.Id);
        modelBuilder.Entity<Defect>().HasKey(o => o.Id);
        modelBuilder.Entity<Manufacturer>().HasKey(o => o.Id);
        modelBuilder.Entity<Owner>().HasKey(o => o.Id);
        modelBuilder.Entity<Vehicle>().HasKey(o => o.Id);
    }
    
    public DbSet<BrandOfVehicle> BrandsOfVehicle { get; set; }
    
    public DbSet<Defect> Defects { get; set; }
    
    public DbSet<Manufacturer> Manufacturers { get; set; }
    
    public DbSet<Owner> Owners { get; set; }    
    
    public DbSet<Vehicle> Vehicles { get; set; }
}