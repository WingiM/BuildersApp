namespace BuildersApp.Core.Models.UserInfo;

[Serializable]
public class PersonalInfoBase
{
    public string Name { get; init; }
    public string Ogrn { get; init; }
    public string Tin { get; init; }
    public string Kpp { get; init; }
    public string Address { get; init; }
}