using System.Text;
using ServiceStation.DataTransferObjects.Implementation;
using ServiceStation.Models.Entities.Implementation;
using ServiceStation.Services.Mapping.Abstraction;

namespace ServiceStation.Services.Mapping.Implementation;

public class OwnerMapper : IMapper<Owner, OwnerDto>
{
    public OwnerDto MapToDto(Owner source)
    {
        var fullName = new StringBuilder();
        
        if (string.IsNullOrWhiteSpace(source.MiddleName))
        {
            fullName.Append(source.FirstName[0]);
            fullName.Append('.');
            fullName.Append(' ');
            fullName.Append(source.LastName);
        }
        else
        {
            fullName.Append(source.LastName);
            fullName.Append(' ');
            fullName.Append(source.FirstName[0]);
            fullName.Append('.');
            fullName.Append(source.MiddleName[0]);
            fullName.Append('.');
        }

        var owner = new OwnerDto
        {
            Id = source.Id,
            FirstName = source.FirstName,
            LastName = source.LastName,
            MiddleName = source.MiddleName,
            FullName = fullName.ToString(),
            City = source.City,
            Street = source.Street,
            BuildingNumber = source.BuildingNumber,
        };
        
        return owner;
    }

    public Owner MapToEntity(OwnerDto destination)
    {
        throw new NotImplementedException();
    }
}