using System.Text;
using ServiceStation.DataTransferObjects.Implementation;
using ServiceStation.Models.Entities.Implementation;
using ServiceStation.Services.Mapping.Abstraction;

namespace ServiceStation.Services.Mapping.Implementation;

public class WorkerMapper : IMapper<Worker, WorkerDto>
{
    public WorkerDto MapToDto(Worker source)
    {
        var fullName = new StringBuilder();
        
        if (string.IsNullOrWhiteSpace(source.MiddleName))
        {
            fullName.Append(source.FirstName[0]);
            fullName.Append('.');
            fullName.Append(source.LastName);
            fullName.Append(' ');
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
        
        var workerDto = new WorkerDto
        {
            Id = source.Id,
            //VehicleId = source.VehicleId,
            LastName = source.LastName,
            FirstName = source.FirstName,
            MiddleName = source.MiddleName,
            FullName = fullName.ToString()
        };
        
        return workerDto;
    }

    public Worker MapToEntity(WorkerDto destination)
    {
        throw new NotImplementedException();
    }
}