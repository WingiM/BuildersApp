using BuildersApp.Core.Enums;

namespace BuildersApp.Core.Models;

public class User
{
    public int Id { get; init; }
    public string Login { get; init; } = null!;
    public string EncryptedPassword { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public Roles Role { get; init; }
    public Data Data { get; init; } = null!;
}