using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ServiceStation.Models.DTOs.Implementation;
using ServiceStation.Models.Entities.Implementation;
using ServiceStation.Repository.Abstraction;
using ServiceStation.Services.Mapping.Abstraction;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.ViewModels.Implementation;

public class DefectDetailsViewModel : AbstractViewModel
{
    private const string EditIcon = "/Resources/Images/edit.png";
    private const string AttemptIcon = "/Resources/Images/check.png";
    
    private DefectDto? _defect;

    private readonly ILogger<DefectDetailsViewModel> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper<Defect, DefectDto> _defectMapper;
    
    private string? _fault;
    private bool _isFaultInputEnabled;
    private string _faultButtonIconPath;
    
    private string? _description;
    private bool _isDescriptionInputEnabled;
    private string _descriptionButtonIconPath;

    private bool _isFixed;
    private string _isFixedColor;
    
    public DefectDetailsViewModel(ILogger<DefectDetailsViewModel> logger, IUnitOfWork unitOfWork,
        IMapper<Defect, DefectDto> defectMapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _defectMapper = defectMapper;
        
        _faultButtonIconPath = EditIcon;
        _descriptionButtonIconPath = EditIcon;

        ChangeInputFaultCommand = new AsyncRelayCommand(ChangeInputFaultEnabled);
        ChangeInputDescriptionCommand = new AsyncRelayCommand(ChangeInputDescriptionEnabled);
    }
    
    public ICommand ChangeInputFaultCommand { get; init; }
    
    public ICommand ChangeInputDescriptionCommand { get; init; }

    public string? Fault
    {
        get => _fault;
        set => SetField(ref _fault, value);
    }

    public bool IsFaultInputEnabled
    {
        get => _isFaultInputEnabled;
        set => SetField(ref _isFaultInputEnabled, value);
    }

    public string FaultButtonIconPath
    {
        get => _faultButtonIconPath;
        set => SetField(ref _faultButtonIconPath, value);
    }

    /*public string ChangeFaultIcon
    {
        get => _changeFaultIcon;
        set => SetField(ref _changeFaultIcon, value);
    }*/

    public string? Description
    {
        get => _description;
        set => SetField(ref _description, value);
    }

    public bool IsDescriptionInputEnabled
    {
        get => _isDescriptionInputEnabled;
        set => SetField(ref _isDescriptionInputEnabled, value);
    }
    
    public string DescriptionButtonIconPath
    {
        get => _descriptionButtonIconPath;
        set => SetField(ref _descriptionButtonIconPath, value);
    }

    public bool IsFixed
    {
        get => _isFixed;
        set => SetField(ref _isFixed, value);
    }

    public string IsFixedColor
    {
        get => _isFixedColor;
        set => SetField(ref _isFixedColor, value);
    }

    public async Task UpdateAsync(Guid defectId)
    {
        var defect = await _unitOfWork.DefectsRepository.GetByIdAsync(defectId);
        if (defect == null) throw new KeyNotFoundException();
        var defectDto = _defectMapper.MapToDto(defect);
        
        _defect = defectDto;
        
    }

    private void UpdateDefect(DefectDto defect)
    {
        Fault = defect.Fault;
        Description = defect.Description;
        IsFixed = defect.IsFixed;
        IsFixedColor = defect.BackgroundColor;
    }
    
    private async Task ChangeInputFaultEnabled()
    {
        if (IsFaultInputEnabled)
        {
            IsFaultInputEnabled = false;
            FaultButtonIconPath = EditIcon;
            //await VerifyFaultChange();
        }
        else
        {
            IsFaultInputEnabled = true;
            FaultButtonIconPath = AttemptIcon;
        }
    }
    
    private async Task VerifyFaultChange()
    {
        if (Fault is null || _defect is null) throw new NullReferenceException(nameof(_defect));
        
        if (!Fault.Equals(_defect.Fault))
        {
            var defectForUpdate = await _unitOfWork.DefectsRepository.GetByIdAsync(_defect.Id);
            if (defectForUpdate is null) throw new KeyNotFoundException();
            defectForUpdate.Fault = Fault;
            await _unitOfWork.DefectsRepository.UpdateAsync(defectForUpdate);

            //TODO мб перед выходом делать
            await _unitOfWork.SaveChangesAsync();
        }
    }
    
    private async Task ChangeInputDescriptionEnabled()
    {
        if (IsDescriptionInputEnabled)
        {
            IsDescriptionInputEnabled = false;
            DescriptionButtonIconPath = EditIcon;
            //await VerifyDescriptionChange();
        }
        else
        {
            IsDescriptionInputEnabled = true;
            DescriptionButtonIconPath = AttemptIcon;
        }
    }
    
    private async Task VerifyDescriptionChange()
    {
        if (Description is null || _defect is null) throw new NullReferenceException(nameof(_defect));
        
        if (!Description.Equals(_defect.Fault))
        {
            var defectForUpdate = await _unitOfWork.DefectsRepository.GetByIdAsync(_defect.Id);
            if (defectForUpdate is null) throw new KeyNotFoundException();
            defectForUpdate.Description = Description;
            await _unitOfWork.DefectsRepository.UpdateAsync(defectForUpdate);

            //TODO мб перед выходом делать
            await _unitOfWork.SaveChangesAsync();
        }
    }
}