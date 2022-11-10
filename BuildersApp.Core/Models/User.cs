using BuildersApp.Core.Enums;

namespace BuildersApp.Core.Models;

public class User
{
    public int Id { get; init; }
    public string Login { get; init; }
    public string EncryptedPassword { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
    public Roles Role { get; init; }
    public Data Data { get; init; }
}