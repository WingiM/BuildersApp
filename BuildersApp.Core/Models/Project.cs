using System.ComponentModel.DataAnnotations;
using BuildersApp.Core.Enums;

namespace BuildersApp.Core.Models;

public class Project
{
    public int Id { get; set; }

    [Required]
    [MinLength(10, ErrorMessage = "Name is too short")]
    public string Name { get; set; }

    public IndustryTypes IndustryType { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Designer is required")]
    public int DesignerId { get; set; }

    public UserTuple? Designer { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Developer is required")]
    public int DeveloperId { get; set; }

    public UserTuple? Developer { get; set; }
    public int CreatedById { get; set; }
    public UserTuple? CreatedBy { get; set; }
    public DateTime DateCreated { get; set; }

    public List<Document>? Documents { get; init; }
}