using System.ComponentModel.DataAnnotations;
using BuildersApp.Core.Enums;

namespace BuildersApp.Core.Models.UserInfo;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [MinLength(5, ErrorMessage = "Login is too short")]
    public string Login { get; set; } = null!;
    public string EncryptedPassword { get; set; } = null!;
    
    [Required]
    [EmailAddress(ErrorMessage = "Email address required")]
    public string Email { get; set; } = null!;
    
    [Required]
    [Phone(ErrorMessage = "Phone number required")]
    public string PhoneNumber { get; set; } = null!;

    [Required] public Roles Role { get; set; } = Roles.Customer;
    public PersonalInfoBase? PersonalInfo { get; set; }
}