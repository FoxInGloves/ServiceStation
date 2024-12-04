using ServiceStation.DataTransferObjects.Abstraction;
using ServiceStation.Models.Entities.Abstraction;
using ServiceStation.Services.Mapping.Abstraction;

namespace ServiceStation.Services.Mapping.Implementation;

public static class MapperExtensions
{
    public static IEnumerable<TDestination> MapToDtos<TSource, TDestination>(
        this IMapper<TSource, TDestination> mapper, 
        IEnumerable<TSource> sources) where TSource : AbstractEntity where TDestination : AbstractDto
    {
        return sources.Select(mapper.MapToDto);
    }

    public static IEnumerable<TSource> MapToEntities<TSource, TDestination>(
        this IMapper<TSource, TDestination> mapper, 
        IEnumerable<TDestination> destinations) where TSource : AbstractEntity where TDestination : AbstractDto
    {
        return destinations.Select(mapper.MapToEntity);
    }
}
