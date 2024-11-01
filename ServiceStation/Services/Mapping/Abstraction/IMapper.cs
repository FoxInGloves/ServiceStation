using ServiceStation.Models.DTOs.Abstraction;
using ServiceStation.Models.Entities.Abstraction;

namespace ServiceStation.Services.Mapping.Abstraction;

//TODO сделать lheujq маппер
public interface IMapper<TSource, TDestination> where TSource : AbstractEntity where TDestination : AbstractDto
{
    TDestination MapToDto(TSource source);
    
    TSource MapToEntity(TDestination destination);
}
