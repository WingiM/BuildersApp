using System.ComponentModel.DataAnnotations;

namespace BuildersApp.Core.Models.UserInfo;

[Serializable]
public abstract class ExtendedPersonalInfo : PersonalInfoBase
{
    [StringLength(maximumLength: 13, MinimumLength = 13, ErrorMessage = "Must be 13 symbols long")]
    public string Ogrn { get; set; }

    [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "Must be 10 symbols long")]
    public string Tin { get; set; }

    [StringLength(maximumLength:9, MinimumLength = 9, ErrorMessage = "Must be 9 symbols long")]
    public string Kpp { get; set; }
    public string Address { get; set; }
}