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
    private readonly IMapper<Vehicle, VehicleDto> _mapper;
    private readonly INavigationService _navigationService;

    private ObservableCollection<VehicleDto>? _collectionOfVehicles;
    private VehicleDto? _selectedVehicle;

    public VehiclesViewModel(ILogger<VehiclesViewModel> logger, IUnitOfWork unitOfWork,
        IMapper<Vehicle, VehicleDto> mapper, INavigationService navigationService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
        var vehicleDto = _mapper.MapToDtos(vehicles);

        CollectionOfVehicles = new ObservableCollection<VehicleDto>(vehicleDto);
        string a;
        string b;
    }

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

        var model = vehicle.ModelOfVehicle;
        if (model is null)
            throw new NullReferenceException(nameof(model));
        var brand = model.Brand;
        if (brand is null)
            throw new NullReferenceException(nameof(brand));

        var brandsOnDb = await _unitOfWork.BrandsOfVehicleRepository.GetAsync();
        var dbBrand =
            brandsOnDb.FirstOrDefault(x => x.Name.Equals(brand.Name, StringComparison.CurrentCultureIgnoreCase));

        Task? addBrandTask;
        if (dbBrand == null)
        {
            addBrandTask = _unitOfWork.BrandsOfVehicleRepository.CreateAsync(brand);
        }
        else
        {
            addBrandTask = Task.CompletedTask;
            model.BrandId = brand.Id;
            model.Brand = brand;
        }

        var modelsOnDb = await _unitOfWork.ModelsRepository.GetAsync();
        var dbModel = modelsOnDb.FirstOrDefault(x =>
            x.Name.Equals(model.Name, StringComparison.CurrentCultureIgnoreCase) &&
            x.BrandId == model.BrandId);
        Task? addModelTask;
        if (dbModel == null)
        {
            addModelTask = _unitOfWork.ModelsRepository.CreateAsync(model);
        }
        else
        {
            addModelTask = Task.CompletedTask;
            vehicle.ModelOfVehicleId = dbModel.Id;
            vehicle.ModelOfVehicle = dbModel;
        }

        var addVehicleTask = _unitOfWork.VehicleRepository.CreateAsync(vehicle);
        var saveChangesTask = _unitOfWork.SaveChangesAsync();

        var vehicleDto = _mapper.MapToDto(vehicle);
        CollectionOfVehicles!.Add(vehicleDto);

        await addOwnerTask;
        await addModelTask;
        await addBrandTask;
        await addVehicleTask;
        await saveChangesTask;
    }

    private ResultT<Vehicle> OpenAddNewVehicleWindow()
    {
        var newVehicleViewAndViewModelResult = _navigationService.NavigateToWindow<AddNewVehicleViewModel>();

        if (!newVehicleViewAndViewModelResult.IsSuccess)
        {
            return Error.Failure(newVehicleViewAndViewModelResult.Error?.Code!, "Could not open new vehicle window");
        }

        var newWorkerViewAndViewModel = newVehicleViewAndViewModelResult.Value;
        newWorkerViewAndViewModel.Item1.Owner = Application.Current.MainWindow;

        var dialog = newWorkerViewAndViewModel.Item1.ShowDialog();

        if (dialog is not null)
            return Error.Failure("WindowClosed", "New vehicle window closed");

        if (newWorkerViewAndViewModel.Item2 is not AddNewVehicleViewModel newVehicleViewModel)
        {
            return Error.Failure("NullReference", "Could not cast AbstractViewModel to AddNewVehicleViewModel");
        }

        var vehicle = newVehicleViewModel.GetVehicle();

        return ResultT<Vehicle>.Success(vehicle);
    }

    private async Task DeleteVehicleAsync(Guid vehicleId)
    {
        //TODO Может тут выкидывать исключение или try catch
        /*if (vehicleId is null)
            return;*/
        
        var confirmation = MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo);

        if (confirmation != MessageBoxResult.Yes) return;

        //TODO откоментить удаление ТС
        /*_logger.LogInformation("delete {defect}", vehicleId);
        await _unitOfWork.DefectsRepository.DeleteAsync(vehicleId);*/
    }

    private async Task NavigateToVehicleDetails()
    {
        var vehicleDetailsResult = await OpenVehicleDetailWindow();
        
        if (!vehicleDetailsResult.IsSuccess)
            _logger.LogError("Failed open vehicle detail window.\nCode: {Code}\nDescription: {Description}",
                vehicleDetailsResult.Error?.Code, vehicleDetailsResult.Error?.Description);
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

        await vehicleDetailsViewModel.Update(SelectedVehicle!.Id);

        var dialog = vehicleDetailsWindowAndViewModel.Item1.ShowDialog();

        if (dialog is not null)
            return Error.Failure("WindowClosed", "Vehicle details window closed");

        return ResultT<bool>.Success(true);
    }
}