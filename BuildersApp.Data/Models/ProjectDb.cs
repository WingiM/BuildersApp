namespace BuildersApp.Data.Models;

public class ProjectDb
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int IndustryId { get; init; }
    public int DesignerId { get; set; }
    public int DeveloperId { get; set; }
    public int CreatedById { get; set; }
    public DateTime DateCreated { get; set; }
}