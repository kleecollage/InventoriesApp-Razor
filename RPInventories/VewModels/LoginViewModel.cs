using System.ComponentModel.DataAnnotations;

namespace RPInventarios.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "User account is required.")]
    [Display(Name = "Account")]
    public string Username { get; set; }

    [Required(ErrorMessage = "User password is required.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}
