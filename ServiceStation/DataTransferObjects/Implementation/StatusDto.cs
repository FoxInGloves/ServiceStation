using System.Windows.Media;
using ServiceStation.DataTransferObjects.Abstraction;

namespace ServiceStation.DataTransferObjects.Implementation;

public class StatusDto : AbstractDto
{
    public string Name { get; set; }
    
    public string Color { get; set; }
    
    //public Brush ColorBrush { get; set; }
}