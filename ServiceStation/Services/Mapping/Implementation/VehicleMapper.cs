using System.Collections;
using ServiceStation.DataTransferObjects.Implementation;
using ServiceStation.Models.Entities.Implementation;
using ServiceStation.Services.Mapping.Abstraction;

namespace ServiceStation.Services.Mapping.Implementation;

public class VehicleMapper : IMapper<Vehicle, VehicleDto>
{
    private readonly IMapper<Owner, OwnerDto> _ownerMapper = new OwnerMapper();
    private readonly IMapper<Worker, WorkerDto> _workerMapper = new WorkerMapper();
    private readonly IMapper<Defect, DefectDto> _defectMapper = new DefectMapper();

    public VehicleDto MapToDto(Vehicle source)
    {
        var owner = _ownerMapper.MapToDto(source.Owner!);

        IEnumerable<WorkerDto>? collectionOfWorkers = null;
        IEnumerable<DefectDto>? collectionOfDefects = null;

        if (source.CollectionOfWorkers is not null)
        {
            collectionOfWorkers = _workerMapper.MapToDtos(source.CollectionOfWorkers);
        }

        if (source.CollectionsOfDefects is not null)
        {
            collectionOfDefects = _defectMapper.MapToDtos(source.CollectionsOfDefects);
        }

        var vehicleDto = new VehicleDto
        {
            Id = source.Id,
            BrandAndModel = source.ModelOfVehicle?.Brand?.Name + " " + source.ModelOfVehicle?.Name,
            RegistrationNumber = source.RegistrationNumber,
            OwnerId = owner.Id,
            StatusId = source.StatusId,
            Status = source.Status,
            YearOfRelease = source.YearOfRelease,
            ServiceCallDate = source.ServiceCallDate,
            ElapsedDays = DateOnly.FromDateTime(DateTime.Today.Date).DayNumber - source.ServiceCallDate.DayNumber,
            ModelOfVehicle = source.ModelOfVehicle,
            Owner = owner,
            OwnerName = owner.FullName,
            CollectionOfWorkers = collectionOfWorkers,
            CollectionsOfDefects = collectionOfDefects
        };

        return vehicleDto;
    }

    public Vehicle MapToEntity(VehicleDto destination)
    {
        var vehicle = new Vehicle
        {
            Id = destination.Id,
            OwnerId = destination.OwnerId,
            StatusId = destination.StatusId,
            ModelOfVehicle = destination.ModelOfVehicle,
            YearOfRelease = destination.YearOfRelease,
            RegistrationNumber = destination.RegistrationNumber,
            ServiceCallDate = destination.ServiceCallDate,
        };

        return vehicle;
    }
}