using System.Globalization;
using ServiceStation.Models.Entities.Implementation;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.ViewModels.Implementation;

public class AddNewVehicleViewModel : AbstractViewModel
{
    public required string VehicleBrand { get; set; }
    public string VehicleModel { get; set; }
    public string YearOfRealese { get; set; }
    public string LicenseNumber { get; set; }

    public string FullName { get; set; }

    public string City { get; set; }
    public string Street { get; set; }
    public string BuildingNumber { get; set; }

    public Vehicle GetVehicle()
    {
        var owner = GetOwner();
        var brand = new BrandOfVehicle
        {
            Name = VehicleBrand
        };
        var model = new ModelOfVehicle
        {
            BrandId = brand.Id,
            Brand = brand,
            Name = VehicleModel
        };

        var vehicle = new Vehicle
        {
            OwnerId = owner.Id,
            ModelOfVehicleId = model.Id,
            YearOfRelease = YearOfRealese,
            RegistrationNumber = LicenseNumber,
            ServiceCallDate = DateTime.Today.ToString(CultureInfo.InvariantCulture),
            Owner = owner,
            ModelOfVehicle = model
        };
        
        return vehicle;
    }
    
    private Owner GetOwner()
    {
        //TODO по человечески парсить имя
        var name = FullName.Trim().Split(" ");

        var middleName = name.Length > 2 ? name[3] : string.Empty;

        var owner = new Owner
        {
            FirstName = name[0],
            LastName = name[1],
            MiddleName = middleName,
            City = City,
            Street = Street,
            BuildingNumber = BuildingNumber
        };
        
        return owner;
    }
}