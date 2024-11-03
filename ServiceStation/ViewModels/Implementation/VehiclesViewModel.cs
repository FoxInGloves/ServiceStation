using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using ServiceStation.Models.DTOs.Implementation;
using ServiceStation.Models.Entities.Implementation;
using ServiceStation.Repository.Abstraction;
using ServiceStation.Services.Mapping.Abstraction;
using ServiceStation.Services.Mapping.Implementation;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.ViewModels.Implementation;

public sealed class VehiclesViewModel : AbstractViewModel
{
    private readonly ILogger<VehiclesViewModel> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper<Vehicle, VehicleDto> _mapper;
    
    private ObservableCollection<VehicleDto>? _collectionOfVehicles;
    
    public VehiclesViewModel(ILogger<VehiclesViewModel> logger, IUnitOfWork unitOfWork, 
        IMapper<Vehicle, VehicleDto> mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public ObservableCollection<VehicleDto> CollectionOfVehicles
    {
        get => _collectionOfVehicles ?? throw new NullReferenceException();
        
        set => SetField(ref _collectionOfVehicles, value);
    }

    public override async Task UpdateAsync()
    {
        //TODO разобраться нужно ли создавать каждый раз новую коллекцию
        var vehicles = await _unitOfWork.VehicleRepository.GetAsync();
        CollectionOfVehicles = new ObservableCollection<VehicleDto>(_mapper.MapToDtos(vehicles));
    }
}