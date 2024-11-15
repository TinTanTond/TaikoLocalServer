using Domain.Enums;

namespace Domain.Models;

public class PlaySetting
{
    public uint Speed { get; set; }

    public bool IsVanishOn { get; set; }
    
    public bool IsInverseOn { get; set; }
    
    public RandomType RandomType { get; set; }
}