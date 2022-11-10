namespace BuildersApp.Core.Models.UserInfo;

[Serializable]
public class DesignerPersonalInfo : PersonalInfoBase
{
    public string Director { get; init; }
    public string LeadEngineer { get; init; }
}