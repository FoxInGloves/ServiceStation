using ServiceStation.Models.DTOs.Implementation;
using ServiceStation.Models.Entities.Implementation;
using ServiceStation.Services.Mapping.Abstraction;
using ServiceStation.Services.XamlConverters;

namespace ServiceStation.Services.Mapping.Implementation;

public class StatusMapper : IMapper<Status, StatusDto>
{
    private readonly StringToBrushConverter _stringToBrushConverter = new ();
    public StatusDto MapToDto(Status source)
    {
        var statusDto = new StatusDto
        {
            Id = source.Id,
            Name = source.Name,
            Color = source.Color,
        };
        
        return statusDto;
    }

    public Status MapToEntity(StatusDto destination)
    {
        throw new NotImplementedException();
    }
}