using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ServiceStation.Models.DTOs.Implementation;
using ServiceStation.Models.Entities.Implementation;
using ServiceStation.Repository.Abstraction;
using ServiceStation.Services.Mapping.Abstraction;
using ServiceStation.Services.Mapping.Implementation;
using ServiceStation.Services.Navigation.Abstraction;
using ServiceStation.Services.ResultT.Abstraction;
using ServiceStation.Services.ResultT.Implementation;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.ViewModels.Implementation;

public sealed class VehiclesViewModel : AbstractViewModel
{
    private readonly ILogger<VehiclesViewModel> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper<Vehicle, VehicleDto> _vehicleMapper;
    private readonly INavigationService _navigationService;

    private ObservableCollection<VehicleDto>? _collectionOfVehicles;
    private VehicleDto? _selectedVehicle;

    public VehiclesViewModel(ILogger<VehiclesViewModel> logger, IUnitOfWork unitOfWork,
        IMapper<Vehicle, VehicleDto> mapper, INavigationService navigationService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _vehicleMapper = mapper;
        _navigationService = navigationService;

        AddNewVehicleAsyncCommand = new AsyncRelayCommand(AddNewVehicle);
        NavigateToVehicleDetailsCommand =
            new AsyncRelayCommand(NavigateToVehicleDetails, () => SelectedVehicle != null);
        DeleteVehicleCommandAsync = new AsyncRelayCommand<Guid>(DeleteVehicleAsync);

        _logger.LogInformation("VehiclesViewModel initialized");
    }

    public ICommand AddNewVehicleAsyncCommand { get; init; }
    
    public ICommand DeleteVehicleCommandAsync { get; init; }

    public ICommand NavigateToVehicleDetailsCommand { get; init; }

    public ObservableCollection<VehicleDto>? CollectionOfVehicles
    {
        get => _collectionOfVehicles;
        set => SetField(ref _collectionOfVehicles, value);
    }

    public VehicleDto? SelectedVehicle
    {
        get => _selectedVehicle;
        set => SetField(ref _selectedVehicle, value);
    }

    public override async Task UpdateAsync()
    {
        _logger.LogInformation("Updating vehicles");
        //TODO разобраться нужно ли создавать каждый раз новую коллекцию
        var vehicles = await _unitOfWork.VehicleRepository.GetAsync();
        var vehicleDto = _vehicleMapper.MapToDtos(vehicles);

        CollectionOfVehicles = new ObservableCollection<VehicleDto>(vehicleDto);
        string a;
        string b;
    }

    //TODO рефактор метода
    private async Task AddNewVehicle()
    {
        var newVehicleResult = OpenAddNewVehicleWindow();

        if (!newVehicleResult.IsSuccess)
        {
            _logger.LogError("Failed open new workers page.\nCode: {Code}\nDescription: {Description}",
                newVehicleResult.Error?.Code, newVehicleResult.Error?.Description);
            return;
        }

        var vehicle = newVehicleResult.Value;

        var owner = vehicle.Owner;
        if (owner is null) throw new ArgumentNullException(nameof(owner));
        var addOwnerTask = _unitOfWork.OwnersRepository.CreateAsync(owner);

        var modelOfVehicle = vehicle.ModelOfVehicle;
        if (modelOfVehicle is null)
            throw new NullReferenceException(nameof(modelOfVehicle));
        
        var brand = modelOfVehicle.Brand;
        if (brand is null)
            throw new NullReferenceException(nameof(brand));

        var brandsOnDb = await _unitOfWork.BrandsOfVehicleRepository.GetAsync();
        var existingBrand =
            brandsOnDb.FirstOrDefault(x => x.Name.Equals(brand.Name, StringComparison.CurrentCultureIgnoreCase));

        Task addBrandTask;
        if (existingBrand == null)
        {
            addBrandTask = _unitOfWork.BrandsOfVehicleRepository.CreateAsync(brand);
        }
        else
        {
            addBrandTask = Task.CompletedTask;
            modelOfVehicle.BrandId = existingBrand.Id;
            modelOfVehicle.Brand = existingBrand;
        }

        var modelsOnDb = await _unitOfWork.ModelsRepository.GetAsync();
        var existingModel = modelsOnDb.FirstOrDefault(x =>
            x.Name.Equals(modelOfVehicle.Name, StringComparison.CurrentCultureIgnoreCase) &&
            x.BrandId == modelOfVehicle.BrandId);
        Task addModelTask;
        if (existingModel == null)
        {
            addModelTask = _unitOfWork.ModelsRepository.CreateAsync(modelOfVehicle);
        }
        else
        {
            addModelTask = Task.CompletedTask;
            vehicle.ModelOfVehicleId = existingModel.Id;
            vehicle.ModelOfVehicle = existingModel;
        }

        var addVehicleTask = _unitOfWork.VehicleRepository.CreateAsync(vehicle);

        var vehicleDto = _vehicleMapper.MapToDto(vehicle);
        CollectionOfVehicles!.Add(vehicleDto);

        await Task.WhenAll(addOwnerTask, addModelTask,  addBrandTask, addVehicleTask);
        
        await _unitOfWork.SaveChangesAsync();
    }

    private ResultT<Vehicle> OpenAddNewVehicleWindow()
    {
        var newVehicleViewAndViewModelResult = _navigationService.NavigateToWindow<AddNewVehicleViewModel>();

        if (!newVehicleViewAndViewModelResult.IsSuccess)
        {
            return Error.Failure(newVehicleViewAndViewModelResult.Error?.Code!, "Could not open new vehicle window");
        }

        var newVehicleViewAndViewModel = newVehicleViewAndViewModelResult.Value;
        newVehicleViewAndViewModel.Item1.Owner = Application.Current.MainWindow;

        var dialog = newVehicleViewAndViewModel.Item1.ShowDialog();

        if (dialog is false)
            return Error.Information("WindowClosed", "New vehicle window closed");

        if (newVehicleViewAndViewModel.Item2 is not AddNewVehicleViewModel newVehicleViewModel)
        {
            return Error.Failure("NullReference", "Could not cast AbstractViewModel to AddNewVehicleViewModel");
        }

        var vehicle = newVehicleViewModel.GetVehicle();

        return ResultT<Vehicle>.Success(vehicle);
    }

    private async Task DeleteVehicleAsync(Guid vehicleId)
    {
        var confirmation = MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo);

        if (confirmation != MessageBoxResult.Yes) return;
        
        var vehicle = await _unitOfWork.VehicleRepository.GetByIdAsync(vehicleId);
        if (vehicle is null) return;
        
        /*var defects = await _unitOfWork.DefectsRepository.GetAsync(x => x.VehicleId.Equals(vehicleId));
        _unitOfWork.DefectsRepository.DeleteRange(defects); */
        
        //var deleteOwnerTask = _unitOfWork.OwnersRepository.DeleteByIdAsync(vehicle.OwnerId);
        var deleteVehicleTask = _unitOfWork.VehicleRepository.DeleteByIdAsync(vehicleId);
        CollectionOfVehicles?.Remove(CollectionOfVehicles.First(x => x.Id == vehicleId));
        
        //await _unitOfWork.DefectsRepository.DeleteByIdAsync(vehicleId);

        //await deleteOwnerTask;
        await deleteVehicleTask;
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task NavigateToVehicleDetails()
    {
        var vehicleDetailsResult = await OpenVehicleDetailWindow();
        
        if (!vehicleDetailsResult.IsSuccess)
            _logger.LogError("Failed open vehicle detail window.\nCode: {Code}\nDescription: {Description}",
                vehicleDetailsResult.Error?.Code, vehicleDetailsResult.Error?.Description);
        
        var vehicle = await _unitOfWork.VehicleRepository.GetByIdAsync(SelectedVehicle!.Id);
        if (vehicle == null) throw new NullReferenceException("Vehicle not found");
        
        var vehicleDto = _vehicleMapper.MapToDto(vehicle);
        
        var vehicleDtoInCollection = CollectionOfVehicles!.First(x => x.Id == vehicle.Id);
        var vehicleDtoIndex = CollectionOfVehicles!.IndexOf(vehicleDtoInCollection);
        
        //CollectionOfVehicles!.Insert(vehicleDtoIndex, vehicleDto);
        CollectionOfVehicles![vehicleDtoIndex] = vehicleDto;
        OnPropertyChanged(nameof(CollectionOfVehicles));
    }

    private async Task<ResultT<bool>> OpenVehicleDetailWindow()
    {
        var vehicleDetails = _navigationService.NavigateToWindow<VehicleDetailsViewModel>();

        if (!vehicleDetails.IsSuccess)
        {
            return Error.Failure(vehicleDetails.Error?.Code!, "Could not open vehicle details window");
        }

        var vehicleDetailsWindowAndViewModel = vehicleDetails.Value;
        vehicleDetailsWindowAndViewModel.Item1.Owner = Application.Current.MainWindow;

        if (vehicleDetailsWindowAndViewModel.Item2 is not VehicleDetailsViewModel vehicleDetailsViewModel)
        {
            return Error.Failure("NullReference", "Could not cast AbstractViewModel to VehicleDetailsViewModel");
        }

        await vehicleDetailsViewModel.UpdateAsync(SelectedVehicle!.Id);

        vehicleDetailsWindowAndViewModel.Item1.ShowDialog();
        
        /*var vehicleToUpdate = 
        await _unitOfWork.VehicleRepository.UpdateAsync()*/

        return ResultT<bool>.Success(true);
    }
}