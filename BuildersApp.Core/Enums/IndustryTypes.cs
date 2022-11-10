using System.ComponentModel.DataAnnotations;

namespace BuildersApp.Core.Enums;

public enum IndustryTypes
{
    [Display(Name = "Любой")] Any = 1,
    [Display(Name = "Водоснабжение")] WaterSupply = 2,
    [Display(Name = "Газификация")] GasSupply = 3
}