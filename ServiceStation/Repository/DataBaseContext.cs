using Microsoft.EntityFrameworkCore;
using ServiceStation.Models.Entities.Implementation;

namespace ServiceStation.Repository;

public class DataBaseContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Database.EnsureCreated();
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