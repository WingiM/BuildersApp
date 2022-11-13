using System.ComponentModel.DataAnnotations;

namespace BuildersApp.Core.Models.UserInfo;

[Serializable]
public class DesignerPersonalInfo : ExtendedPersonalInfo
{
    [Required]
    public string Director { get; set; }
    
    [Required]
    public string LeadEngineer { get; set; }
}