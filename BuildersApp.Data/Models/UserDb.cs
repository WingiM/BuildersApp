using System.Text.Json;
using System.Text.Json.Nodes;
using BuildersApp.Core.Enums;
using BuildersApp.Core.Models;

namespace BuildersApp.Data.Models;

public class UserDb
{
    public int Id { get; init; }
    public string Login { get; init; }
    public string EncryptedPassword { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
    public int RoleId { get; init; }
    public string Data { get; init; }

    public Core.Models.Data GetData() =>
        RoleId switch
        {
            (int)Roles.Customer => new Core.Models.Data(),
            (int)Roles.Designer => JsonSerializer.Deserialize<DesignerData>(Data) ?? throw new Exception(),
            (int)Roles.Developer => JsonSerializer.Deserialize<DeveloperData>(Data) ?? throw new Exception(),
            _ => throw new Exception()
        };
}