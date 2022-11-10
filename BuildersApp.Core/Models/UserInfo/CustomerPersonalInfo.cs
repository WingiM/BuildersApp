using BuildersApp.Core.Enums;

namespace BuildersApp.Core.Models.UserInfo;

[Serializable]
public class CustomerPersonalInfo : PersonalInfoBase
{
    public IndustryTypes IndustryType { get; init; }
}