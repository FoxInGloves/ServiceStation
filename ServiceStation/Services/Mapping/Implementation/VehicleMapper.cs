using ServiceStation.Models.DTOs.Implementation;
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
        
        return new VehicleDto
        {
            Id = source.Id,
            BrandAndModel = source.ModelOfVehicle?.Brand?.Name + " " + source.ModelOfVehicle?.Name,
            RegistrationNumber = source.RegistrationNumber,
            StatusId = source.StatusId,
            Status = source.Status,
            YearOfRelease = source.YearOfRelease,
            ServiceCallDate = source.ServiceCallDate,
            ElapsedTime = GetDays(source.ServiceCallDate, DateTime.Today.Date.ToString()),
            ModelOfVehicle = source.ModelOfVehicle,
            Owner = owner,
            OwnerName = owner.FullName,
            CollectionOfWorkers = _workerMapper.MapToDtos(source.CollectionOfWorkers!),
            CollectionsOfDefects = _defectMapper.MapToDtos(source.CollectionsOfDefects!)
        };
    }

    public Vehicle MapToEntity(VehicleDto destination)
    {
        throw new NotImplementedException();
    }

    private string GetDays(string dateOne, string dateTwo)
    {
        var start = DateTime.Parse(dateOne);
        var end = DateTime.Parse(dateTwo);
        
        return (end - start).TotalDays.ToString();
    }
}