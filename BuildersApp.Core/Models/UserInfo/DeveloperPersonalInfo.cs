using System.ComponentModel.DataAnnotations;

namespace BuildersApp.Core.Models.UserInfo;

[Serializable]
public class DeveloperPersonalInfo : ExtendedPersonalInfo
{
    [Required]
    public string ExecutiveDirector { get; set; }
}