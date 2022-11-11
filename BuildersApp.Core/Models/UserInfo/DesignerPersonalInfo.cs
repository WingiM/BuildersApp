namespace BuildersApp.Core.Models.UserInfo;

[Serializable]
public class DesignerPersonalInfo : ExtendedPersonalInfo
{
    public string Director { get; init; }
    public string LeadEngineer { get; init; }
}