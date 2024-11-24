using System.Windows.Media;
using ServiceStation.Models.DTOs.Abstraction;

namespace ServiceStation.Models.DTOs.Implementation;

public class StatusDto : AbstractDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Color { get; set; }
    
    public Brush ColorBrush { get; set; }
}