using System.ComponentModel.DataAnnotations;

namespace Ensure.Entities.Domain;

public class Login
{
    [Required(ErrorMessage = "Email required")]
    [EmailAddress]
    public string email { get; set; }
    [Required(ErrorMessage = "Password required")]
    public string password { get; set; }
}