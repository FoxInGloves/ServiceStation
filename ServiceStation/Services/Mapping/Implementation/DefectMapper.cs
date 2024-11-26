using ServiceStation.Models.DTOs.Implementation;
using ServiceStation.Models.Entities.Implementation;
using ServiceStation.Services.Mapping.Abstraction;

namespace ServiceStation.Services.Mapping.Implementation;

public class DefectMapper : IMapper<Defect, DefectDto>
{
    public DefectDto MapToDto(Defect source)
    {
        var defectDto = new DefectDto
        {
            Id = source.Id,
            VehicleId = source.VehicleId,
            WorkerId = source.WorkerId,
            Fault = source.Fault,
            Description = source.Description,
            IsFixed = source.IsFixed,
            BackgroundColor = source.IsFixed ? "#02b83e" : "#f52020",
            /*StartDate = source.StartDate,
            EndDate = source.EndDate*/
        };
        
        return defectDto;
    }

    public Defect MapToEntity(DefectDto destination)
    {
        throw new NotImplementedException();
    }
}