namespace BuildersApp.Core.Models;

public record LoginCredentials
{
    public string Login { get; set; } = null!;
    public string? Password { get; set; } = null!;
}