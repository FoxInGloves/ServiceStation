using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ServiceStation.Models.DTOs.Implementation;
using ServiceStation.Models.Entities.Implementation;
using ServiceStation.Repository.Abstraction;
using ServiceStation.Services.Mapping.Abstraction;
using ServiceStation.Services.Navigation.Abstraction;
using ServiceStation.Services.ResultT.Abstraction;
using ServiceStation.Services.ResultT.Implementation;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.ViewModels.Implementation;

public class VehicleDetailsViewModel : AbstractViewModel
{
    private readonly ILogger<VehicleDetailsViewModel> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INavigationService _navigationService;
    private readonly IMapper<Vehicle, VehicleDto> _vehicleMapper;

    private VehicleDto? _vehicle;
    private OwnerDto? _owner;
    private ModelOfVehicle? _model;

    private const string EditIcon = "/Resources/Images/edit.png";
    private const string AttemptIcon = "/Resources/Images/check.png";
    private string _iconPath = EditIcon;
    private bool _isInputEnabled;

    public VehicleDetailsViewModel(ILogger<VehicleDetailsViewModel> logger, IUnitOfWork unitOfWork,
        INavigationService navigationService, IMapper<Vehicle, VehicleDto> vehicleMapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _navigationService = navigationService;
        _vehicleMapper = vehicleMapper;
        //RevertIsFixedStatusOfDefectCommand = new AsyncRelayCommand<string>();

        ChangeInputRegistrationNumberEnabledCommand = new AsyncRelayCommand(ChangeInputRegistrationNumberEnabled);
        NavigateToOwnerDetailsWindowCommand = new AsyncRelayCommand<string>(NavigateToOwnerDetailsWindow);
    }

    public ICommand RevertIsFixedStatusOfDefectCommand { get; init; }

    public ICommand DeleteDefectCommandAsync { get; init; }
    
    public ICommand NavigateToOwnerDetailsWindowCommand { get; init; }

    public ICommand ChangeInputRegistrationNumberEnabledCommand { get; init; }
    
    public string? BrandAndModel { get; private set; }

    public string? YearOfRelease { get; private set; }

    public string? RegistrationNumber { get; set; }

    public string? ServiceCallDate { get; private set; }

    public Status? SelectedStatus { get; set; }
    
    public ObservableCollection<Status>? CollectionOfStatuses { get; set; }
    
    public string? OwnerId { get; private set; }

    public string? FullNameOwner { get; private set; }

    public ObservableCollection<DefectDto>? Defects { get; set; }

    public string IconPath
    {
        get => _iconPath;
        set
        {
            _iconPath = value;
            OnPropertyChanged();
        }
    }

    public bool IsInputEnabled
    {
        get => _isInputEnabled;
        set
        {
            _isInputEnabled = value;
            OnPropertyChanged();
        }
    }

    public async Task Update(Guid vehicleId)
    {
        var vehicle = await _unitOfWork.VehicleRepository.GetByIdAsync(vehicleId);
        if (vehicle == null) throw new KeyNotFoundException();
        var vehicleDto = _vehicleMapper.MapToDto(vehicle);

        _vehicle = vehicleDto;
        _owner = vehicleDto.Owner ?? throw new NullReferenceException(nameof(vehicle.Owner));
        _model = vehicle.ModelOfVehicle ?? throw new NullReferenceException(nameof(vehicle.ModelOfVehicle));

        var updateVehicleTask = UpdateVehicle();
        UpdateOwner();
        
        await updateVehicleTask;

        for (var i = 0; i < 18; i++)
        {
            var def = new[]
            {
                new DefectDto
                {
                    Fault = $"Defect{i}",
                    IsFixed = false,
                    BackgroundColor = "#f52020"
                },
                new DefectDto
                {
                    Fault = $"Defect{i}",
                    IsFixed = true,
                    BackgroundColor = "#02b83e"
                }
            };

            var random = new Random();
            random.Shuffle(def);

            Defects?.Add(def[0]);
        }
    }

    private async Task UpdateVehicle()
    {
        if (_vehicle is null) throw new NullReferenceException(nameof(_vehicle));
        
        BrandAndModel = _vehicle.BrandAndModel;
        YearOfRelease = _vehicle.YearOfRelease;
        RegistrationNumber = _vehicle.RegistrationNumber;
        ServiceCallDate = _vehicle.ServiceCallDate;
        SelectedStatus = _vehicle.Status;
        CollectionOfStatuses = new ObservableCollection<Status>(await _unitOfWork.StatusRepository.GetAsync());
        if (_vehicle.CollectionsOfDefects != null)
            Defects = new ObservableCollection<DefectDto>(_vehicle.CollectionsOfDefects);
    }

    private void UpdateOwner()
    {
        OwnerId = _owner?.Id.ToString();
        FullNameOwner = _owner?.FullName ?? throw new NullReferenceException(nameof(_owner.FullName));
    }

    private void RevertIsFixedStatusOfDefect(Guid defectId)
    {
        //defect.IsFixed = true;
    }

    private async Task DeleteDefect(Guid defectId)
    {
        var confirmation = MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo);

        if (confirmation != MessageBoxResult.Yes) return;

        _logger.LogInformation("delete {defect}", defectId);
        await _unitOfWork.DefectsRepository.DeleteAsync(defectId);
    }

    private async Task NavigateToOwnerDetailsWindow(string? ownerId)
    {
        if (string.IsNullOrEmpty(ownerId)) throw new NullReferenceException(nameof(ownerId));
        var result = await OpenOwnerDetailsWindow(new Guid(ownerId));
    }
    
    private async Task<ResultT<bool>> OpenOwnerDetailsWindow(Guid ownerId)
    {
        var vehicleDetails = _navigationService.NavigateToWindow<OwnerDetailsViewModel>();

        if (!vehicleDetails.IsSuccess)
        {
            return Error.Failure(vehicleDetails.Error?.Code!, "Could not open vehicle details window");
        }

        var ownerDetailsWindowAndViewModel = vehicleDetails.Value;
        ownerDetailsWindowAndViewModel.Item1.Owner = Application.Current.MainWindow;

        if (ownerDetailsWindowAndViewModel.Item2 is not OwnerDetailsViewModel ownerDetailsViewModel)
        {
            return Error.Failure("NullReference", "Could not cast AbstractViewModel to VehicleDetailsViewModel");
        }

        await ownerDetailsViewModel.UpdateAsync(ownerId);

        var dialog = ownerDetailsWindowAndViewModel.Item1.ShowDialog();

        if (dialog is not null)
            return Error.Failure("WindowClosed", "Vehicle details window closed");

        return ResultT<bool>.Success(true);
    }

    private async Task ChangeInputRegistrationNumberEnabled()
    {
        if (IsInputEnabled)
        {
            IsInputEnabled = false;
            IconPath = EditIcon;
            await VerifyRegistrationNumberChange();
        }
        else
        {
            IsInputEnabled = true;
            IconPath = AttemptIcon;
        }
    }

    private async Task VerifyRegistrationNumberChange()
    {
        if (RegistrationNumber is null || _vehicle is null) throw new NullReferenceException(nameof(_vehicle));
        
        if (!RegistrationNumber.Equals(_vehicle.RegistrationNumber))
        {
            var vehicleForUpdate = await _unitOfWork.VehicleRepository.GetByIdAsync(_vehicle.Id);
            if (vehicleForUpdate is null) throw new KeyNotFoundException();
            vehicleForUpdate.RegistrationNumber = RegistrationNumber;
            await _unitOfWork.VehicleRepository.UpdateAsync(vehicleForUpdate);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}