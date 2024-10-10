using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EnsureFreightInc.Entities.Domain;

namespace Ensure.Entities.Domain;

public class NewUser 
{
    public User user { get; set; }
    [Required(ErrorMessage = "Password required")]
    [MaxLength(20,ErrorMessage = "Password can not be more than {1} characters")]
    [MinLength(6,ErrorMessage = "Password can not be less than {1} characters")]
    [PasswordPropertyText]
    public string password { get; set; } = string.Empty;
}