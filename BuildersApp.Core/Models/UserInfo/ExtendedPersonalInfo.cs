namespace BuildersApp.Core.Models.UserInfo;

[Serializable]
public abstract class ExtendedPersonalInfo : PersonalInfoBase
{
    public string Ogrn { get; init; }
    public string Tin { get; init; }
    public string Kpp { get; init; }
    public string Address { get; init; }
}