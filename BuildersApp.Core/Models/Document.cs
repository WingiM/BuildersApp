namespace BuildersApp.Core.Models;

public class Document
{
    public int Id { get; init; }
    public string Name { get; init; }
    public bool IsSigned { get; set; }
    public bool IsNecessary { get; set; }
}