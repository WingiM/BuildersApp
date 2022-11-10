using BuildersApp.Core.Enums;

namespace BuildersApp.Core.Models;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IndustryTypes IndustryType { get; init; }
    public UserTuple Designer { get; set; }
    public UserTuple Developer { get; set; }
    public UserTuple CreatedBy { get; set; }
    public DateTime DateCreated { get; set; }
    
    public List<Document> Documents { get; }
}