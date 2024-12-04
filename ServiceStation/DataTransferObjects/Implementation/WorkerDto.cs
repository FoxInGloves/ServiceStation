using ServiceStation.DataTransferObjects.Abstraction;

namespace ServiceStation.DataTransferObjects.Implementation;

public class WorkerDto : AbstractDto
{
    public Guid DefectId { get; set; }
    
    public string FullName { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string? MiddleName { get; set; }
}