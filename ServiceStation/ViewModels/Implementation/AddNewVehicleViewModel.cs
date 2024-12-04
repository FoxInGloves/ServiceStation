using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ServiceStation.Models.Entities.Implementation;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.ViewModels.Implementation;

public class AddNewVehicleViewModel : AbstractViewModel
{
    private readonly ILogger<AddNewVehicleViewModel> _logger;

    private string _vehicleBrand;
    private string _vehicleModel;
    private int _yearOfRelease;
    //private string 
    
    public AddNewVehicleViewModel(ILogger<AddNewVehicleViewModel> logger)
    {
        _logger = logger;
        
        CloseWindowCommand = new RelayCommand<(Window, bool)>(CloseWindow);
    }
    
    public ICommand CloseWindowCommand { get; init; }

    public string VehicleBrand
    {
        get => _vehicleBrand;
        set => SetField(ref _vehicleBrand, value);
    }
    public string VehicleModel { get; set; }
    public string YearOfRelease { get; set; }
    public string RegistrationNumber { get; set; }

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
            YearOfRelease = Convert.ToInt32(YearOfRelease),
            RegistrationNumber = RegistrationNumber,
            ServiceCallDate = DateOnly.FromDateTime(DateTime.Today),
            Owner = owner,
            ModelOfVehicle = model
        };
        
        return vehicle;
    }
    
    private Owner GetOwner()
    {
        var name = FullName.Trim().Split(' ');
        var middleName = name.Length > 2 ? name[2] : string.Empty;

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

    private void CloseWindow((Window? WindowToClose, bool DialogResult) parameters)
    {
        if (parameters.WindowToClose is null)
        {
            _logger.LogError("Parameter AddNewVehicle window is null");
            return;
        }
        
        if (parameters.DialogResult is false)
        {
            parameters.WindowToClose.Close();
            return;
        }
        
        if (IsNullableFields())
        {
            MessageBox.Show("Одно из обязательных полей (*) незаполнено");
            return;
        }
        
        parameters.WindowToClose.DialogResult = true;
    }

    private bool IsNullableFields()
    {
        var isNullableFields = string.IsNullOrWhiteSpace(VehicleBrand)
                               || string.IsNullOrWhiteSpace(VehicleModel)
                               || string.IsNullOrWhiteSpace(RegistrationNumber)
                               || (string.IsNullOrWhiteSpace(FullName) && FullName.Split(' ').Length < 2)
                               || string.IsNullOrWhiteSpace(City)
                               || string.IsNullOrWhiteSpace(Street)
                               || string.IsNullOrWhiteSpace(BuildingNumber);
        
        return isNullableFields;
    }
}