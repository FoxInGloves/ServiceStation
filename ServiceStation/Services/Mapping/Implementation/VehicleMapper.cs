using ServiceStation.Models.DTOs.Implementation;
using ServiceStation.Models.Entities.Implementation;
using ServiceStation.Services.Mapping.Abstraction;

namespace ServiceStation.Services.Mapping.Implementation;

public class VehicleMapper : IMapper<Vehicle, VehicleDto>
{
    public VehicleDto MapToDto(Vehicle source)
    {
        return new VehicleDto
        {
            Id = source.Id,
            BrandAndModel = source.BrandOfVehicle.Brand + " " + source.BrandOfVehicle.Model,
            LicenseNumber = source.LicenseNumber,
            Status = source.Status,
            YearOfRelease = source.YearOfRelease
        };
    }

    public Vehicle MapToEntity(VehicleDto destination)
    {
        throw new NotImplementedException();
    }
}