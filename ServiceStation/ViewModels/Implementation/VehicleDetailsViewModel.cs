using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ServiceStation.Models.DTOs.Implementation;
using ServiceStation.Models.Entities.Implementation;
using ServiceStation.Repository.Abstraction;
using ServiceStation.Services;
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
    private readonly IMapper<Defect, DefectDto> _defectMapper;

    private VehicleDto? _vehicle;
    private OwnerDto? _owner;
    private ModelOfVehicle? _model;
    
    private Status? _selectedStatus;
    
    private const string EditIcon = "/Resources/Images/edit.png";
    private const string AttemptIcon = "/Resources/Images/check.png";
    private string _iconPath;
    private bool _isInputEnabled;

    public VehicleDetailsViewModel(ILogger<VehicleDetailsViewModel> logger, IUnitOfWork unitOfWork,
        INavigationService navigationService, IMapper<Vehicle, VehicleDto> vehicleMapper, IMapper<Defect, DefectDto> defectMapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _navigationService = navigationService;
        _vehicleMapper = vehicleMapper;
        _defectMapper = defectMapper;

        _iconPath = EditIcon;

        CloseWindowCommand = new RelayCommand<Window>(CloseWindow);

        RevertIsFixedStatusOfDefectCommand = new AsyncRelayCommand<Guid>(RevertIsFixedStatusOfDefect);
        
        AddNewDefectAsyncCommand = new AsyncRelayCommand(AddNewDefect);
        DeleteDefectAsyncCommand = new AsyncRelayCommand<Guid>(DeleteDefectAsync);
        
        NavigateToOwnerDetailsWindowAsyncCommand = new AsyncRelayCommand<string>(NavigateToOwnerDetailsWindow);
        NavigateToDefectDetailsAsyncCommand = new AsyncRelayCommand<Guid>(NavigateToDefectDetailsWindow);

        ChangeInputRegistrationNumberEnabledAsyncCommand = new AsyncRelayCommand(ChangeInputRegistrationNumberEnabledAsync);
        
        StatusChangedAsyncCommand = new AsyncRelayCommand(VerifyStatusChangeAsync);
    }

    public ICommand CloseWindowCommand { get; init; } 
    
    public ICommand RevertIsFixedStatusOfDefectCommand { get; init; }

    public ICommand DeleteDefectAsyncCommand { get; init; }
    
    public ICommand AddNewDefectAsyncCommand { get; init; }
    
    public ICommand NavigateToOwnerDetailsWindowAsyncCommand { get; init; }
    
    public ICommand NavigateToDefectDetailsAsyncCommand { get; init; }

    public ICommand ChangeInputRegistrationNumberEnabledAsyncCommand { get; init; }
    
    public ICommand StatusChangedAsyncCommand { get; init; }
    
    public string? BrandAndModel { get; private set; }

    public int YearOfRelease { get; private set; }

    public string? RegistrationNumber { get; set; }

    public DateOnly ServiceCallDate { get; private set; }

    public Status? SelectedStatus
    {
        get => _selectedStatus;
        set => SetField(ref _selectedStatus, value);
    }
    
    public ObservableCollection<Status>? CollectionOfStatuses { get; set; }
    
    public string? OwnerId { get; private set; }

    public string? FullNameOwner { get; private set; }

    private ObservableCollection<DefectDto>? _collectionOfDefects;

    public ObservableCollection<DefectDto>? CollectionOfDefects /*{ get; set; }*/
    {
        get => _collectionOfDefects;
        set => SetField(ref _collectionOfDefects, value);
    }

    public string IconPath
    {
        get => _iconPath;
        set => SetField(ref _iconPath, value);
    }

    public bool IsInputEnabled
    {
        get => _isInputEnabled;
        set => SetField(ref _isInputEnabled, value);
    }

    public async Task UpdateAsync(Guid vehicleId)
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

        //TODO убрать заглушку генерации данных
        /*for (var i = 0; i < 18; i++)
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
        }*/
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
            CollectionOfDefects = new ObservableCollection<DefectDto>(_vehicle.CollectionsOfDefects);
    }

    private void UpdateOwner()
    {
        OwnerId = _owner?.Id.ToString();
        FullNameOwner = _owner?.FullName ?? throw new NullReferenceException(nameof(_owner.FullName));
    }

    private static void CloseWindow(Window? windowToClose)
    {
        if (windowToClose != null) windowToClose.DialogResult = false;
    }

    private async Task RevertIsFixedStatusOfDefect(Guid defectId)
    {
        var defect = await _unitOfWork.DefectsRepository.GetByIdAsync(defectId);
        if (defect == null) throw new KeyNotFoundException();

        defect.IsFixed = !defect.IsFixed;
        
        var updateDefectTask = _unitOfWork.DefectsRepository.UpdateAsync(defect);
        
        var defectDtoToUpdate = CollectionOfDefects!.First(d => d.Id == defect.Id);
        var indexDefectToUpdate = CollectionOfDefects!.IndexOf(defectDtoToUpdate);
        
        defectDtoToUpdate.IsFixed = defect.IsFixed;
        defectDtoToUpdate.BackgroundColor = EntityColorService.GetStatusColor(defect.IsFixed);
        
        CollectionOfDefects[indexDefectToUpdate] = defectDtoToUpdate;
        /*CollectionOfDefects[a].BackgroundColor = EntityColorService.GetStatusColor(defect.IsFixed);
        CollectionOfDefects[a].IsFixed = defect.IsFixed;*/
        
        OnPropertyChanged(nameof(CollectionOfDefects));
        
        await updateDefectTask;
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task AddNewDefect()
    {
        var result = OpenAddNewDefectWindow();
        
        if (!result.IsSuccess)
        {
            _logger.LogError("Failed to open details details window. \nCode: {Code}\nDescription: {Description}",
                result.Error?.Code, result.Error?.Description);
            return;
        }

        var defect = result.Value;
        
        var addDefectTask = _unitOfWork.DefectsRepository.CreateAsync(defect);
        CollectionOfDefects!.Add(_defectMapper.MapToDto(defect));

        await addDefectTask;
        await _unitOfWork.SaveChangesAsync();
    }

    private ResultT<Defect> OpenAddNewDefectWindow()
    {
        var addNewDefectResult = _navigationService.NavigateToWindow<AddNewDefectViewModel>();

        if (!addNewDefectResult.IsSuccess)
        {
            return Error.Failure(addNewDefectResult.Error?.Code!, "Could not open vehicle details window");
        }

        var addNewDefectWindowAndViewModel = addNewDefectResult.Value;
        addNewDefectWindowAndViewModel.Item1.Owner = Application.Current.MainWindow;
        
        var dialog = addNewDefectWindowAndViewModel.Item1.ShowDialog();
        
        if (dialog is false)
            return Error.Information("WindowClosed", "Vehicle details window closed");
        
        if (addNewDefectWindowAndViewModel.Item2 is not AddNewDefectViewModel addNewDefectViewModel)
        {
            return Error.Failure("NullReference", "Could not cast AbstractViewModel to VehicleDetailsViewModel");
        }

        var defect = new Defect
        {
            VehicleId = _vehicle!.Id,
            Fault = addNewDefectViewModel.Fault!,
            Description = addNewDefectViewModel.Description,
            IsFixed = false,
            //StartDate = DateTime.Now,
        };
        

        return ResultT<Defect>.Success(defect);
    }
    
    private async Task DeleteDefectAsync(Guid defectId)
    {
        var confirmation = MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo);

        if (confirmation != MessageBoxResult.Yes) return;

        _logger.LogInformation("delete {defect}", defectId);
        
        CollectionOfDefects!.Remove(CollectionOfDefects!.First(d => d.Id == defectId));
        
        await _unitOfWork.DefectsRepository.DeleteByIdAsync(defectId);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task NavigateToOwnerDetailsWindow(string? ownerId)
    {
        if (string.IsNullOrEmpty(ownerId)) throw new NullReferenceException(nameof(ownerId));
        var result = await OpenOwnerDetailsWindow(new Guid(ownerId));

        if (!result.IsSuccess)
        {
            _logger.LogError("Failed to open owner details window. \nCode: {Code}\nDescription: {Description}",
                result.Error?.Code, result.Error?.Description);
        }
    }
    
    private async Task<ResultT<bool>> OpenOwnerDetailsWindow(Guid ownerId)
    {
        var vehicleDetailsResult = _navigationService.NavigateToWindow<OwnerDetailsViewModel>();

        if (!vehicleDetailsResult.IsSuccess)
        {
            return Error.Failure(vehicleDetailsResult.Error?.Code!, "Could not open vehicle details window");
        }

        var ownerDetailsWindowAndViewModel = vehicleDetailsResult.Value;
        ownerDetailsWindowAndViewModel.Item1.Owner = Application.Current.MainWindow;

        if (ownerDetailsWindowAndViewModel.Item2 is not OwnerDetailsViewModel ownerDetailsViewModel)
        {
            return Error.Failure("NullReference", "Could not cast AbstractViewModel to VehicleDetailsViewModel");
        }

        await ownerDetailsViewModel.UpdateAsync(ownerId);

        ownerDetailsWindowAndViewModel.Item1.ShowDialog();

        return ResultT<bool>.Success(true);
    }
    
    private async Task NavigateToDefectDetailsWindow(Guid defectId)
    {
        var result = await OpenDefectDetailsWindow(defectId);
        
        if (!result.IsSuccess)
        {
            _logger.LogError("Failed to open defect details window. \nCode: {Code}\nDescription: {Description}",
                result.Error?.Code, result.Error?.Description);
        }
    }
    
    private async Task<ResultT<bool>> OpenDefectDetailsWindow(Guid defectId)
    {
        var defectDetailsResult = _navigationService.NavigateToWindow<DefectDetailsViewModel>();

        if (!defectDetailsResult.IsSuccess)
        {
            return Error.Failure(defectDetailsResult.Error?.Code!, "Could not open vehicle details window");
        }

        var defectDetailsWindowAndViewModel = defectDetailsResult.Value;
        defectDetailsWindowAndViewModel.Item1.Owner = Application.Current.MainWindow;

        if (defectDetailsWindowAndViewModel.Item2 is not DefectDetailsViewModel defectDetailsViewModel)
        {
            return Error.Failure("NullReference", "Could not cast AbstractViewModel to VehicleDetailsViewModel");
        }

        await defectDetailsViewModel.UpdateAsync(defectId);

        var dialog = defectDetailsWindowAndViewModel.Item1.ShowDialog();

        if (dialog is false)
            return Error.Information("WindowClosed", "Vehicle details window closed");

        return ResultT<bool>.Success(true);
    }

    private async Task ChangeInputRegistrationNumberEnabledAsync()
    {
        if (IsInputEnabled)
        {
            IsInputEnabled = false;
            IconPath = EditIcon;
            await VerifyRegistrationNumberChangeAsync();
        }
        else
        {
            IsInputEnabled = true;
            IconPath = AttemptIcon;
        }
    }

    private async Task VerifyRegistrationNumberChangeAsync()
    {
        if (RegistrationNumber is null || _vehicle is null) throw new NullReferenceException();
        
        if (!RegistrationNumber.Equals(_vehicle.RegistrationNumber))
        {
            var vehicleForUpdate = await _unitOfWork.VehicleRepository.GetByIdAsync(_vehicle.Id);
            if (vehicleForUpdate is null) throw new KeyNotFoundException(nameof(vehicleForUpdate));
            
            vehicleForUpdate.RegistrationNumber = RegistrationNumber;
            _vehicle.RegistrationNumber = RegistrationNumber;
            
            await _unitOfWork.VehicleRepository.UpdateAsync(vehicleForUpdate);

            await _unitOfWork.SaveChangesAsync();
        }
    }
    
    private async Task VerifyStatusChangeAsync()
    {
        if (SelectedStatus is null || _vehicle is null) throw new NullReferenceException();

        if (!SelectedStatus.Equals(_vehicle.Status))
        {
            var vehicleForUpdate = await _unitOfWork.VehicleRepository.GetByIdAsync(_vehicle.Id);
            if (vehicleForUpdate is null) throw new KeyNotFoundException(nameof(vehicleForUpdate));
            
            vehicleForUpdate.Status = SelectedStatus;
            _vehicle.Status = SelectedStatus;
            
            await _unitOfWork.VehicleRepository.UpdateAsync(vehicleForUpdate);
            
            await _unitOfWork.SaveChangesAsync();
        }
    }
}