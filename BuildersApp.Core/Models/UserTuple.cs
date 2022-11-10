using BuildersApp.Core.Enums;

namespace BuildersApp.Core.Models;

public class UserTuple
{
    public int Id { get; init; }
    public string Name { get; init; }
    public Roles Role { get; init; }
}