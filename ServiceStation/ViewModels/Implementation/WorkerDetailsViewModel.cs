using System.Text;
using Microsoft.Extensions.Logging;
using ServiceStation.DataTransferObjects.Implementation;
using ServiceStation.Models.Entities.Implementation;
using ServiceStation.Repository.Abstraction;
using ServiceStation.Services.Mapping.Abstraction;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.ViewModels.Implementation;

public class WorkerDetailsViewModel : AbstractViewModel
{
    private readonly ILogger<OwnerDetailsViewModel> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper<Worker, WorkerDto> _workerMapper;
    
    public WorkerDetailsViewModel(ILogger<OwnerDetailsViewModel> logger, IUnitOfWork unitOfWork,
        IMapper<Worker, WorkerDto> workerMapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _workerMapper = workerMapper;
    }
    
    public string? FullName { get; private set; }
    
    public async Task UpdateAsync(Guid workerId)
    {
        var worker = await _unitOfWork.WorkersRepository.GetByIdAsync(workerId);
        if (worker is null) throw new NullReferenceException(nameof(worker));
        var workerDto = _workerMapper.MapToDto(worker);
        UpdateBindingProperties(workerDto);
    }
    
    private void UpdateBindingProperties(WorkerDto workerDto)
    {
        if (workerDto is null) throw new NullReferenceException(nameof(workerDto));
        
        var fullName = new StringBuilder();
        
        if (string.IsNullOrWhiteSpace(workerDto.MiddleName))
        {
            fullName.Append(workerDto.FirstName);
            fullName.Append(' ');
            fullName.Append(workerDto.LastName);
        }
        else
        {
            fullName.Append(workerDto.LastName);
            fullName.Append(' ');
            fullName.Append(workerDto.FirstName);
            fullName.Append(' ');
            fullName.Append(workerDto.MiddleName);
        }
        
        FullName = fullName.ToString();
    }
}