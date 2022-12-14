using System.Text.Json;
using BuildersApp.Core.Enums;
using BuildersApp.Core.Models.UserInfo;

namespace BuildersApp.Data.Models;

public class UserDb
{
    public int Id { get; init; }
    public string Login { get; init; } = null!;
    public string EncryptedPassword { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public int RoleId { get; init; }
    public string Data { get; init; } = null!;

    public PersonalInfoBase? GetData() =>
        RoleId switch
        {
            (int)Roles.Customer => JsonSerializer.Deserialize<CustomerPersonalInfo>(Data),
            (int)Roles.Designer => JsonSerializer.Deserialize<DesignerPersonalInfo>(Data),
            (int)Roles.Developer => JsonSerializer.Deserialize<DeveloperPersonalInfo>(Data),
            _ => throw new Exception()
        };
}