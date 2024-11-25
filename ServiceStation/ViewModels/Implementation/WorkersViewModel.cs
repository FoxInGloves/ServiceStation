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

public class WorkersViewModel : AbstractViewModel
{
    private readonly ILogger<WorkersViewModel> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper<Worker, WorkerDto> _mapper;
    private readonly INavigationService _navigationService;
    
    private ObservableCollection<WorkerDto>? _workers;

    public WorkersViewModel(ILogger<WorkersViewModel> logger, IUnitOfWork unitOfWork,
        IMapper<Worker, WorkerDto> mapper, INavigationService navigationService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        DeleteWorkerAsyncCommand = new AsyncRelayCommand<Guid>(DeleteWorkerAsync);
        AddNewWorkerAsyncCommand = new AsyncRelayCommand(AddNewWorker);
        NavigateToWorkerDetailsCommand = new AsyncRelayCommand<Guid>(NavigateToWorkerDetailsWindow);
        
        CollectionOfWorkers = [];
    }
    
    public ICommand DeleteWorkerAsyncCommand { get; init; }
    
    public ICommand AddNewWorkerAsyncCommand { get; init; }
    
    public ICommand NavigateToWorkerDetailsCommand { get; init; }

    public ObservableCollection<WorkerDto>? CollectionOfWorkers
    {
        get => _workers;
        
        set => SetField(ref _workers, value);
    }

    public override async Task UpdateAsync()
    {
        var workers = await _unitOfWork.WorkersRepository.GetAsync();
        var workersDtos = _mapper.MapToDtos(workers);
        CollectionOfWorkers = new ObservableCollection<WorkerDto>(workersDtos);
    }

    private async Task DeleteWorkerAsync(Guid id)
    {
        var confirmation = MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo);

        if (confirmation != MessageBoxResult.Yes) return;
        
        var deleteWorkerTask = _unitOfWork.WorkersRepository.DeleteByIdAsync(id);
        var saveChangesTask = _unitOfWork.SaveChangesAsync();

        var worker = CollectionOfWorkers!.First(w => w.Id.Equals(id));
        CollectionOfWorkers!.Remove(worker);
        
        await deleteWorkerTask;
        await saveChangesTask;
    }

    private async Task AddNewWorker()
    {
        var newWorkerResult = OpenAddNewWorkerWindow();

        if (!newWorkerResult.IsSuccess)
        {
            _logger.LogError("Failed open new workers page.\nCode: {Code}\nDescription: {Description}",
                newWorkerResult.Error?.Code, newWorkerResult.Error?.Description);
            return;
        }
            
        
        var worker = newWorkerResult.Value;
        
        var createWorkerTask = _unitOfWork.WorkersRepository.CreateAsync(worker);
        var saveChangesTask = _unitOfWork.SaveChangesAsync();
        
        var workerDto = _mapper.MapToDto(worker);
        CollectionOfWorkers!.Add(workerDto);

        await createWorkerTask;
        await saveChangesTask;
    }

    private ResultT<Worker> OpenAddNewWorkerWindow()
    {
        var newWorkerViewAndViewModelResult = _navigationService.NavigateToWindow<AddNewWorkerViewModel>();

        if (!newWorkerViewAndViewModelResult.IsSuccess)
        {
            return Error.Failure(newWorkerViewAndViewModelResult.Error?.Code!, "Could not open new workers page");
        }

        var newWorkerViewAndViewModel = newWorkerViewAndViewModelResult.Value;

        var dialog = newWorkerViewAndViewModel.Item1.ShowDialog();
        
        if (dialog is null)
            return Error.Failure("WindowClosed", "New worker window closed");

        if (newWorkerViewAndViewModel.Item2 is not AddNewWorkerViewModel newWorkerViewModel )
        {
            return Error.Failure("NullReference", "Could not cast AbstractViewModel to AddNewWorkerViewModel");
        }

        var worker = new Worker
        {
            LastName = newWorkerViewModel.LastName!,
            FirstName = newWorkerViewModel.FirstName!,
            MiddleName = newWorkerViewModel.MiddleName,
        };
        
        return ResultT<Worker>.Success(worker);
    }
    
    private async Task NavigateToWorkerDetailsWindow(Guid workerId)
    {
        //if (string.IsNullOrEmpty(workerId)) throw new NullReferenceException(nameof(workerId));
        var result = await OpenWorkerDetailsWindow(workerId);
    }
    
    private async Task<ResultT<bool>> OpenWorkerDetailsWindow(Guid workerId)
    {
        var workerDetails = _navigationService.NavigateToWindow<WorkerDetailsViewModel>();

        if (!workerDetails.IsSuccess)
        {
            _logger.LogError("Could not open vehicle details window.\nCode: {Code}\nDescription: {Description}",
                workerDetails.Error?.Code, workerDetails.Error?.Description);
        }

        var workerDetailsWindowAndViewModel = workerDetails.Value;
        workerDetailsWindowAndViewModel.Item1.Owner = Application.Current.MainWindow;
        
        if (workerDetailsWindowAndViewModel.Item2 is not WorkerDetailsViewModel workerDetailsViewModel)
        {
            return Error.Failure("NullReference", "Could not cast AbstractViewModel to VehicleDetailsViewModel");
        }

        await workerDetailsViewModel.UpdateAsync(workerId);

        var dialog = workerDetailsWindowAndViewModel.Item1.ShowDialog();

        if (dialog is not null)
            return Error.Failure("WindowClosed", "Vehicle details window closed");

        return ResultT<bool>.Success(true);
    }
}