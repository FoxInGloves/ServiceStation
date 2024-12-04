using System.Text;
using Microsoft.Extensions.Logging;
using ServiceStation.DataTransferObjects.Implementation;
using ServiceStation.Models.Entities.Implementation;
using ServiceStation.Repository.Abstraction;
using ServiceStation.Services.Mapping.Abstraction;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.ViewModels.Implementation;

public class OwnerDetailsViewModel : AbstractViewModel
{
    private readonly ILogger<OwnerDetailsViewModel> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper<Owner, OwnerDto> _ownerMapper;
    
    public OwnerDetailsViewModel(ILogger<OwnerDetailsViewModel> logger, IUnitOfWork unitOfWork,
        IMapper<Owner, OwnerDto> ownerMapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _ownerMapper = ownerMapper;
    }
    
    public string? FullName { get; private set; }
    
    public string? City { get; private set; }
    
    public string? Street { get; private set; }
    
    public string? BuildingNumber { get; private set; }
    
    public async Task UpdateAsync(Guid ownerId)
    {
        var owner = await _unitOfWork.OwnersRepository.GetByIdAsync(ownerId);
        if (owner is null) throw new NullReferenceException(nameof(owner));
        var ownerDto = _ownerMapper.MapToDto(owner);
        UpdateBindingProperties(ownerDto);
    }

    private void UpdateBindingProperties(OwnerDto ownerDto)
    {
        if (ownerDto is null) throw new NullReferenceException(nameof(ownerDto));
        
        var fullName = new StringBuilder();
        
        if (string.IsNullOrWhiteSpace(ownerDto.MiddleName))
        {
            fullName.Append(ownerDto.FirstName);
            fullName.Append(' ');
            fullName.Append(ownerDto.LastName);
        }
        else
        {
            fullName.Append(ownerDto.LastName);
            fullName.Append(' ');
            fullName.Append(ownerDto.FirstName);
            fullName.Append(' ');
            fullName.Append(ownerDto.MiddleName);
        }
        
        FullName = fullName.ToString();
        City = ownerDto.City;
        Street = ownerDto.Street;
        BuildingNumber = ownerDto.BuildingNumber;
    }
}