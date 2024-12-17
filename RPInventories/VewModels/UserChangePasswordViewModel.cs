using System.ComponentModel.DataAnnotations;

namespace RPInventories.VewModels;

public class UserChangePasswordViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "User Account")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Password confirmation is required")]
    [MinLength(5, ErrorMessage = "Password must be at least 5 characters"),
     MaxLength(20, ErrorMessage = "Password cannot exceed 20 characters")]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Password confirmation is required")]
    [MinLength(5, ErrorMessage = "Password must be at least 5 characters"),
     MaxLength(20, ErrorMessage = "Password cannot exceed 20 characters")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm New     Password")]
    [Compare("Password", ErrorMessage = "Passwords don't match")]
    public string ConfirmPassword { get; set; }
}