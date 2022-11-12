using BuildersApp.Core.Enums;

namespace BuildersApp.Core.Models.UserInfo;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = null!;
    public string EncryptedPassword { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public Roles Role { get; set; }
    public PersonalInfoBase PersonalInfo { get; set; } = null!;
}