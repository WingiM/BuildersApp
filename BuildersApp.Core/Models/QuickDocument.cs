using System.ComponentModel.DataAnnotations;

namespace BuildersApp.Core.Models;

public class QuickDocument
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Select a document")]
    public int DocumentId { get; set; }
}